using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyChamSocThuCung.Data;
using QuanLyChamSocThuCung.Models;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class HoSoChamSocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoSoChamSocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== DANH SÁCH HỒ SƠ CHĂM SÓC =====
        public IActionResult Index()
        {
            var data = _context.HoSoChamSocs
                .Include(h => h.ThuCung)
                .Include(h => h.LichHen)
                .ThenInclude(l => l.DichVu)
                .ToList();

            return View(data);
        }

        // ===== FORM KHÁM =====
        public IActionResult CapNhat(int lichHenId)
        {
            var lich = _context.LichHens
                .Include(l => l.ThuCung)
                .FirstOrDefault(l => l.LichHenId == lichHenId);

            if (lich == null)
            {
                return NotFound();
            }

            var hoSo = _context.HoSoChamSocs
                .FirstOrDefault(h => h.LichHenId == lichHenId);

            if (hoSo == null)
            {
                hoSo = new HoSoChamSoc
                {
                    LichHenId = lichHenId,
                    ThuCungId = lich.ThuCungId
                };
            }

            return View(hoSo);
        }

        // ===== LƯU KẾT QUẢ KHÁM =====
        [HttpPost]
        public IActionResult CapNhat(HoSoChamSoc hs)
        {
            var hoSo = _context.HoSoChamSocs
                .FirstOrDefault(h => h.LichHenId == hs.LichHenId);

            if (hoSo == null)
            {
                hoSo = new HoSoChamSoc
                {
                    ThuCungId = hs.ThuCungId,
                    LichHenId = hs.LichHenId,
                    ChanDoan = hs.ChanDoan,
                    DieuTri = hs.DieuTri,
                    GhiChu = hs.GhiChu
                };

                _context.HoSoChamSocs.Add(hoSo);
            }
            else
            {
                hoSo.ChanDoan = hs.ChanDoan;
                hoSo.DieuTri = hs.DieuTri;
                hoSo.GhiChu = hs.GhiChu;
            }

            var lich = _context.LichHens.FirstOrDefault(l => l.LichHenId == hs.LichHenId);

            if (lich != null)
            {
                lich.TrangThai = "Hoàn thành";
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "LichHen");
        }

        // ===== KHÁCH HÀNG XEM LỊCH SỬ =====
        public IActionResult LichSuChamSoc()
        {
            var data = _context.HoSoChamSocs
                .Include(h => h.ThuCung)
                .Include(h => h.LichHen)
                .ThenInclude(l => l.DichVu)
                .ToList();

            return View(data);
        }
    }
}