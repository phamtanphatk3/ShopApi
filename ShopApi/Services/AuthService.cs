using Microsoft.IdentityModel.Tokens;
using ShopApi.Common;
using ShopApi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        public async Task<string?> Login(string username, string password)
        {
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT Key is missing");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            var validPassword = PasswordHelper.VerifyPassword(password, user.Password);
            if (!validPassword)
                return null;

            // Upgrade old plaintext password to hashed format after successful login.
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
