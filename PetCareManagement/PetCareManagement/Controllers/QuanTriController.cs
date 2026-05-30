using Microsoft.AspNetCore.Mvc;
using QuanLyChamSocThuCung.Data;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class QuanTriController : Controller
    {

        private readonly ApplicationDbContext _context;

        public QuanTriController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {

            ViewBag.SoThuCung = _context.ThuCungs.Count();

            ViewBag.SoDichVu = _context.DichVus.Count();

            ViewBag.SoLichHen = _context.LichHens.Count();

            ViewBag.SoKhachHang = _context.KhachHangs.Count();

            return View();
        }
        public IActionResult ThongKe()
        {

            var dichvu = _context.LichHens
            .GroupBy(l => l.DichVuId)
            .Select(g => new
            {
                DichVu = g.Key,
                SoLan = g.Count()
            })
            .OrderByDescending(x => x.SoLan)
            .ToList();

            ViewBag.ThongKe = dichvu;

            return View();

        }
    }
}