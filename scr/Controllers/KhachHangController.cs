using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyChamSocThuCung.Data;
using QuanLyChamSocThuCung.Models;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KhachHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách khách hàng
        public IActionResult Index()
        {
            var list = _context.KhachHangs.ToList();
            return View(list);
        }

        // Xem thú cưng của khách
        public IActionResult ThuCung(int id)
        {
            var thuCungs = _context.ThuCungs
                .Where(t => t.KhachHangId == id)
                .ToList();

            ViewBag.KhachHang = _context.KhachHangs
                .FirstOrDefault(k => k.KhachHangId == id);

            return View(thuCungs);
        }
    }
}