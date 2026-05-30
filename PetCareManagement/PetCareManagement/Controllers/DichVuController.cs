using Microsoft.AspNetCore.Mvc;
using QuanLyChamSocThuCung.Data;
using QuanLyChamSocThuCung.Models;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class DichVuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DichVuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách dịch vụ
        public IActionResult Index()
        {
            var list = _context.DichVus.ToList();
            return View(list);
        }

        // Form thêm
        public IActionResult Create()
        {
            return View();
        }

        // Lưu thêm
        [HttpPost]
        public IActionResult Create(DichVu dv)
        {
            if (ModelState.IsValid)
            {
                _context.DichVus.Add(dv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dv);
        }

        // Form sửa
        public IActionResult Edit(int id)
        {
            var dv = _context.DichVus.Find(id);

            if (dv == null)
            {
                return NotFound();
            }

            return View(dv);
        }

        // Lưu sửa
        [HttpPost]
        public IActionResult Edit(DichVu dv)
        {
            if (ModelState.IsValid)
            {
                _context.DichVus.Update(dv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dv);
        }

        // Xóa dịch vụ
        public IActionResult Delete(int id)
        {
            var dv = _context.DichVus.Find(id);

            if (dv == null)
            {
                return NotFound();
            }

            _context.DichVus.Remove(dv);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}