using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyChamSocThuCung.Models
{
    [Table("HoSoChamSoc")]
    public class HoSoChamSoc
    {
        [Key]
        public int HoSoId { get; set; }

        public int ThuCungId { get; set; }

        public int LichHenId { get; set; }

        public string ChanDoan { get; set; }

        public string DieuTri { get; set; }

        public string GhiChu { get; set; }


        [ForeignKey("ThuCungId")]
        public ThuCung ThuCung { get; set; }

        [ForeignKey("LichHenId")]
        public LichHen LichHen { get; set; }
    }
}