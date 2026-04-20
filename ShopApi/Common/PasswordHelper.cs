using System.Security.Cryptography;

namespace ShopApi.Common
{
    // Ham tien ich de hash va xac thuc mat khau.
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;
        private const string Prefix = "PBKDF2";

        // Tao chuoi hash PBKDF2 tu mat khau thuong.
        public static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var key = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize);

            return $"{Prefix}${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        }

        // Kiem tra mat khau dau vao co khop voi mat khau da luu hay khong.
        public static bool VerifyPassword(string password, string storedPassword)
        {
            if (string.IsNullOrWhiteSpace(storedPassword))
                return false;

            if (!storedPassword.StartsWith($"{Prefix}$", StringComparison.Ordinal))
            {
                // Tuong thich nguoc voi du lieu mat khau plaintext cu trong DB.
                return password == storedPassword;
            }

            var parts = storedPassword.Split('$');
            if (parts.Length != 4)
                return false;

            if (!int.TryParse(parts[1], out var iterations))
                return false;

            var salt = Convert.FromBase64String(parts[2]);
            var storedKey = Convert.FromBase64String(parts[3]);

            var key = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                storedKey.Length);

            return CryptographicOperations.FixedTimeEquals(key, storedKey);
        }

        // Kiem tra gia tri luu trong DB da o dinh dang hash hay chua.
        public static bool IsHashed(string storedPassword)
            => !string.IsNullOrWhiteSpace(storedPassword)
               && storedPassword.StartsWith($"{Prefix}$", StringComparison.Ordinal);
    }
}
