using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyChamSocThuCung.Models
{
    /// <summary>
    /// Lưu nhật ký đăng nhập thành công/thất bại để phục vụ yêu cầu kiểm thử bảo mật.
    /// </summary>
    [Table("NhatKyDangNhap")]
    public class NhatKyDangNhap
    {
        [Key]
        public int NhatKyId { get; set; }

        public int? TaiKhoanId { get; set; }

        [StringLength(50)]
        public string? TenDangNhap { get; set; }

        /// <summary>
        /// true: đăng nhập thành công, false: đăng nhập thất bại.
        /// </summary>
        public bool TrangThai { get; set; }

        [StringLength(255)]
        public string? NoiDung { get; set; }

        [StringLength(50)]
        public string? DiaChiIp { get; set; }

        public DateTime ThoiGian { get; set; } = DateTime.Now;
    }
}
