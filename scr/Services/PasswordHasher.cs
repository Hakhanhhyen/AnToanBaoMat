using System.Security.Cryptography;

namespace QuanLyChamSocThuCung.Services
{
    /// <summary>
    /// Xử lý băm mật khẩu bằng PBKDF2 + salt.
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;      // 128-bit salt
        private const int KeySize = 32;       // 256-bit hash
        private const int Iterations = 100000;

        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            //Đây là nơi hệ thống sinh PasswordSalt ngẫu nhiên có độ dài 16 byte (128 bit).
            //Mỗi tài khoản sẽ có một Salt riêng trước khi thực hiện băm mật khẩu bằng PBKDF2.
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            //Sau khi sinh Salt, hệ thống sử dụng thuật toán PBKDF2
            //kết hợp giữa mật khẩu người dùng và Salt để tạo ra PasswordHash.
            //Trong chương trình nhóm em đặt số vòng lặp là 100.000
            //để làm chậm quá trình dò mật khẩu của kẻ tấn công.
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySize
            );
            //Sau khi PBKDF2 xử lý xong, Salt và Hash
            //được chuyển sang chuỗi Base64 để lưu
            //vào hai cột PasswordSalt và PasswordHash trong bảng TaiKhoan.
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
            //Đây là quá trình đăng nhập.
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
