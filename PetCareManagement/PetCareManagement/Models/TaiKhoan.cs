using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyChamSocThuCung.Models
{
    /// <summary>
    /// Model đại diện cho bảng TaiKhoan trong database.
    /// Mật khẩu không còn được lưu dạng rõ, mà được lưu bằng PasswordHash + PasswordSalt.
    /// </summary>
    [Table("TaiKhoan")]
    public class TaiKhoan
    {
        [Key]
        public int TaiKhoanId { get; set; }

        /// <summary>
        /// Tên đăng nhập của người dùng.
        /// </summary>
        [Column("TenDangNhap")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được quá 50 ký tự")]
        public string TenDangNhap { get; set; } = string.Empty;

        /// <summary>
        /// Cột cũ trong database. Không dùng để lưu mật khẩu thật nữa.
        /// Chỉ giữ lại để tránh phá cấu trúc database cũ.
        /// </summary>
        [Column("MatKhau")]
        [StringLength(100)]
        public string MatKhau { get; set; } = "Đã bảo vệ";

        /// <summary>
        /// Chuỗi hash của mật khẩu sau khi xử lý bằng PBKDF2.
        /// </summary>
        [Column("PasswordHash")]
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Salt riêng của từng tài khoản.
        /// </summary>
        [Column("PasswordSalt")]
        public string? PasswordSalt { get; set; }

        /// <summary>
        /// Số lần đăng nhập sai liên tiếp.
        /// </summary>
        [Column("SoLanDangNhapSai")]
        public int SoLanDangNhapSai { get; set; } = 0;

        /// <summary>
        /// Nếu tài khoản bị khóa tạm thời, lưu thời điểm hết khóa.
        /// </summary>
        [Column("ThoiGianKhoa")]
        public DateTime? ThoiGianKhoa { get; set; }

        /// <summary>
        /// Thời điểm đổi mật khẩu gần nhất.
        /// </summary>
        [Column("NgayDoiMatKhau")]
        public DateTime? NgayDoiMatKhau { get; set; }

        /// <summary>
        /// Vai trò của tài khoản: Admin, NhanVien, KhachHang.
        /// </summary>
        [Column("VaiTro")]
        [Required(ErrorMessage = "Vai trò không được để trống")]
        [StringLength(20)]
        public string VaiTro { get; set; } = string.Empty;
    }
}
