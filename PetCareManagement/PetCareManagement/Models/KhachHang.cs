using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyChamSocThuCung.Models
{
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        public int KhachHangId { get; set; }

        public string HoTen { get; set; }

        public string DienThoai { get; set; }

        public string Email { get; set; }

        public ICollection<ThuCung>? ThuCungs { get; set; }
    }
}