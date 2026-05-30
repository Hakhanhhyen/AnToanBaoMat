using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyChamSocThuCung.Models
{
    [Table("LichHen")]
    public class LichHen
    {
        [Key]
        public int LichHenId { get; set; }

        public int ThuCungId { get; set; }
        public int DichVuId { get; set; }
        public DateTime NgayHen { get; set; }

        // Trạng thái: Chờ duyệt, Đã xác nhận, Hoàn thành, Hủy
        public string TrangThai { get; set; } = "Chờ duyệt";

        [ForeignKey("ThuCungId")]
        public ThuCung? ThuCung { get; set; }

        [ForeignKey("DichVuId")]
        public DichVu? DichVu { get; set; }
    }
}