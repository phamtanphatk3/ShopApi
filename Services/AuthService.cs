using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Common;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.DTOs.Auth;
using ShopApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _http;

        public AuthService(AppDbContext context, IConfiguration config, IHttpContextAccessor http)
        {
            _context = context;
            _config = config;
            _http = http;
        }

        public async Task<UserProfileDto?> GetProfileAsync(int userId)
        {
            return await _context.Users
                .Where(x => x.Id == userId)
                .Select(x => new UserProfileDto
                {
                    Id = x.Id,
                    Username = x.Username,
                    Role = x.Role,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserProfileDto> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new AppNotFoundException("Khong tim thay nguoi dung");

            if (!PasswordHelper.VerifyPassword(currentPassword, user.Password))
                throw new AppBadRequestException("Mat khau hien tai khong dung");

            user.Password = PasswordHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return MapProfile(user);
        }

        // Dang ky tai khoan moi.
        public async Task<User> RegisterAsync(string username, string password, string? role = null, string? email = null, string? phone = null, string? address = null)
        {
            username = username.Trim();

            var existed = await _context.Users.AnyAsync(x => x.Username == username);
            if (existed)
                throw new AppConflictException("Ten dang nhap da ton tai");

            var normalizedRole = string.IsNullOrWhiteSpace(role) ? "Customer" : role.Trim();
            if (normalizedRole != "Admin" && normalizedRole != "Staff" && normalizedRole != "Customer")
                throw new AppBadRequestException("Role khong hop le");

            var user = new User
            {
                Username = username,
                Password = PasswordHelper.HashPassword(password),
                Role = normalizedRole,
                Email = email,
                Phone = phone,
                Address = address
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<AuthSessionDto?> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
                return null;

            var validPassword = PasswordHelper.VerifyPassword(password, user.Password);
            if (!validPassword)
                return null;

            if (!PasswordHelper.IsHashed(user.Password))
            {
                user.Password = PasswordHelper.HashPassword(password);
                await _context.SaveChangesAsync();
            }

            return await IssueSessionAsync(user);
        }

        public async Task<AuthSessionDto?> RefreshAsync(string refreshToken)
        {
            var tokenHash = HashToken(refreshToken);
            var tokenEntity = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (tokenEntity == null || !tokenEntity.IsActive)
                return null;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var session = await IssueSessionAsync(tokenEntity.User);

                tokenEntity.UsedAt = DateTime.UtcNow;
                tokenEntity.RevokedAt = DateTime.UtcNow;
                tokenEntity.RevokedReason = "Da lam moi refresh token";
                tokenEntity.RevokedByIp = GetClientIp();
                tokenEntity.ReplacedByTokenHash = HashToken(session.RefreshToken);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return session;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var tokenHash = HashToken(refreshToken);
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (tokenEntity == null || !tokenEntity.IsActive)
                return false;

            tokenEntity.RevokedAt = DateTime.UtcNow;
            tokenEntity.RevokedReason = "Dang xuat";
            tokenEntity.RevokedByIp = GetClientIp();

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<AuthSessionDto> IssueSessionAsync(User user)
        {
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new AppBadRequestException("Thieu cau hinh khoa JWT");

            var refreshDays = _config.GetValue<int?>("Jwt:RefreshTokenDays") ?? 7;
            var jwtId = Guid.NewGuid().ToString("N");
            var accessToken = GenerateAccessToken(user, jwtKey, jwtId);
            var refreshToken = GenerateRefreshToken();
            var refreshEntity = new RefreshToken
            {
                TokenHash = HashToken(refreshToken),
                JwtId = jwtId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshDays),
                CreatedByIp = GetClientIp()
            };

            _context.RefreshTokens.Add(refreshEntity);
            await _context.SaveChangesAsync();

            return new AuthSessionDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshEntity.ExpiresAt,
                User = MapProfile(user)
            };
        }

        private string GenerateAccessToken(User user, string jwtKey, string jwtId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Base64UrlEncoder.Encode(bytes);
        }

        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        private static UserProfileDto MapProfile(User user)
        {
            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address
            };
        }

        private string? GetClientIp()
        {
            var context = _http.HttpContext;
            if (context == null)
                return null;

            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}
