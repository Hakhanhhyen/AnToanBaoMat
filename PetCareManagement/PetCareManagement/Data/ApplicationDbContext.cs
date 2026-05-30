using Microsoft.EntityFrameworkCore;
using QuanLyChamSocThuCung.Models;

namespace QuanLyChamSocThuCung.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DichVu> DichVus { get; set; }

        public DbSet<ThuCung> ThuCungs { get; set; }

        public DbSet<LichHen> LichHens { get; set; }

        public DbSet<HoSoChamSoc> HoSoChamSocs { get; set; }

        public DbSet<KhachHang> KhachHangs { get; set; }

        public DbSet<NhanVien> NhanViens { get; set; }

        public DbSet<TaiKhoan> TaiKhoans { get; set; }

        public DbSet<NhatKyDangNhap> NhatKyDangNhaps { get; set; }
    }
}