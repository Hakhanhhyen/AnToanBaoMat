using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyChamSocThuCung.Models
{
    [Table("NhanVienThuCung")]
    public class NhanVien
    {
        [Key]
        public int NhanVienId { get; set; }

        public string TenNhanVien { get; set; }

        public string DienThoai { get; set; }

        public string ChucVu { get; set; }
    }
}