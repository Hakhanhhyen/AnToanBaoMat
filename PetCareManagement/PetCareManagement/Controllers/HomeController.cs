using Microsoft.AspNetCore.Mvc;
using QuanLyChamSocThuCung.Data;
using Microsoft.EntityFrameworkCore;

namespace QuanLyChamSocThuCung.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Danh sách tất cả dịch vụ
            ViewBag.DichVu = _context.DichVus.ToList();

            // Dịch vụ nổi bật (top 3 giá cao nhất)
            ViewBag.DichVuNoiBat = _context.DichVus
                .OrderByDescending(d => d.Gia)
                .Take(3)
                .ToList();

            return View();
        }
    }
}