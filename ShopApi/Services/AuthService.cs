using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Common;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Dang ky tai khoan moi.
        public async Task<Models.User> RegisterAsync(string username, string password, string? role = null)
        {
            username = username.Trim();

            var existed = await _context.Users.AnyAsync(x => x.Username == username);
            if (existed)
                throw new AppConflictException("Ten dang nhap da ton tai");

            var normalizedRole = string.IsNullOrWhiteSpace(role) ? "Customer" : role.Trim();
            if (normalizedRole != "Admin" && normalizedRole != "Staff" && normalizedRole != "Customer")
                throw new AppBadRequestException("Role khong hop le");

            var user = new Models.User
            {
                Username = username,
                Password = PasswordHelper.HashPassword(password),
                Role = normalizedRole
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // Dang nhap, kiem tra mat khau va tao JWT token cho nguoi dung hop le.
        public async Task<string?> Login(string username, string password)
        {
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new AppBadRequestException("Thieu cau hinh khoa JWT");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            var validPassword = PasswordHelper.VerifyPassword(password, user.Password);
            if (!validPassword)
                return null;

            // Nang cap mat khau cu dang plaintext sang dang hash sau khi dang nhap thanh cong.
            if (!PasswordHelper.IsHashed(user.Password))
            {
                user.Password = PasswordHelper.HashPassword(password);
                await _context.SaveChangesAsync();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


