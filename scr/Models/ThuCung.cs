using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyChamSocThuCung.Models
{
    [Table("ThuCung")]
    public class ThuCung
    {
        [Key]
        public int ThuCungId { get; set; }

        public string TenThuCung { get; set; }

        public string Loai { get; set; }

        public string Giong { get; set; }

        public int Tuoi { get; set; }

        public int KhachHangId { get; set; }

        public KhachHang KhachHang { get; set; }
    }
}