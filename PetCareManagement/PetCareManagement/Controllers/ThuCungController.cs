using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyChamSocThuCung.Data;
using QuanLyChamSocThuCung.Models;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class ThuCungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThuCungController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ten = HttpContext.Session.GetString("TenDangNhap");
            var kh = _context.KhachHangs.FirstOrDefault(k => k.Email == ten);

            var thuCungs = kh != null
                ? _context.ThuCungs.Where(t => t.KhachHangId == kh.KhachHangId).ToList()
                : _context.ThuCungs.ToList(); // Nếu không tìm thấy kh thì hiện hết để demo

            return View(thuCungs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ThuCung tc)
        {
            var ten = HttpContext.Session.GetString("TenDangNhap");
            var kh = _context.KhachHangs.FirstOrDefault(k => k.Email == ten);

            // Gán KhachHangId để tránh lỗi FK
            tc.KhachHangId = kh != null ? kh.KhachHangId : 1; // Nếu không có kh thì gán tạm 1 để lưu

            _context.ThuCungs.Add(tc);
            _context.SaveChanges(); // Lưu luôn, không kiểm tra ModelState
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var tc = _context.ThuCungs.Find(id);
            if (tc == null)
                return NotFound();

            return View(tc);
        }
        [HttpPost]
        public IActionResult Edit(ThuCung tc)
        {
            if (ModelState.IsValid)
            {
                _context.ThuCungs.Update(tc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tc);
        }
        public IActionResult Delete(int id)
        {
            var tc = _context.ThuCungs.Find(id);

            if (tc == null)
                return NotFound();

            _context.ThuCungs.Remove(tc);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}