using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyChamSocThuCung.Models
{
    [Table("DichVu")]
    public class DichVu
    {
        [Key]
        public int DichVuId { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ không được để trống")]
        public string TenDichVu { get; set; }

        [Range(1000, 10000000, ErrorMessage = "Giá không hợp lệ")]
        public decimal Gia { get; set; }

        public string MoTa { get; set; }
    }
}