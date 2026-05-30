using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyChamSocThuCung.Data;
using QuanLyChamSocThuCung.Models;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class LichHenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LichHenController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.LichHens
                .Include(l => l.ThuCung)
                .Include(l => l.DichVu)
                .ToList();

            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.ThuCung = _context.ThuCungs.ToList();
            ViewBag.DichVu = _context.DichVus.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(LichHen lichHen)
        {
            if (ModelState.IsValid)
            {
                lichHen.TrangThai = "Chờ duyệt";
                _context.LichHens.Add(lichHen);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            //  nếu lỗi thì phải load lại ViewBag
            ViewBag.ThuCung = _context.ThuCungs.ToList();
            ViewBag.DichVu = _context.DichVus.ToList();

            return View(lichHen);
        }

        // Admin và Nhân viên đều được thao tác
        public IActionResult Duyet(int id)
        {
            var lich = _context.LichHens.Find(id);
            if (lich != null)
            {
                lich.TrangThai = "Đã duyệt";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult TuChoi(int id)
        {
            var lich = _context.LichHens.Find(id);
            if (lich != null)
            {
                lich.TrangThai = "Từ chối";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult HoanThanh(int id)
        {
            var lich = _context.LichHens.Find(id);
            if (lich != null)
            {
                lich.TrangThai = "Hoàn thành";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}