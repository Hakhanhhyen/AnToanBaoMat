using System.Security.Cryptography;

namespace QuanLyChamSocThuCung.Services
{
    /// <summary>
    /// Xử lý băm mật khẩu bằng PBKDF2 + salt.
    /// Không lưu mật khẩu dạng rõ, không dùng SHA-256 đơn thuần.
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;      // 128-bit salt
        private const int KeySize = 32;       // 256-bit hash
        private const int Iterations = 100000;

        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySize
            );

            passwordSalt = Convert.ToBase64String(salt);
            passwordHash = Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(storedHash) ||
                string.IsNullOrWhiteSpace(storedSalt))
            {
                return false;
            }

            byte[] salt;
            byte[] expectedHash;

            try
            {
                salt = Convert.FromBase64String(storedSalt);
                expectedHash = Convert.FromBase64String(storedHash);
            }
            catch
            {
                return false;
            }

            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySize
            );

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
