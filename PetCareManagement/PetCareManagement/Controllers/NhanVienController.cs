using Microsoft.AspNetCore.Mvc;
using QuanLyChamSocThuCung.Data;
using QuanLyChamSocThuCung.Models;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhanVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách nhân viên
        public IActionResult Index()
        {
            var list = _context.NhanViens.ToList();
            return View(list);
        }

        // Form thêm
        public IActionResult Create()
        {
            return View();
        }

        // Lưu nhân viên
        [HttpPost]
        public IActionResult Create(NhanVien nv)
        {
            if (ModelState.IsValid)
            {
                _context.Set<NhanVien>().Add(nv);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(nv);
        }

        // Form sửa
        public IActionResult Edit(int id)
        {
            var nv = _context.Set<NhanVien>().Find(id);

            if (nv == null)
                return NotFound();

            return View(nv);
        }

        // Lưu sửa
        [HttpPost]
        public IActionResult Edit(NhanVien nv)
        {
            if (ModelState.IsValid)
            {
                _context.Set<NhanVien>().Update(nv);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(nv);
        }

        // Xóa nhân viên
        public IActionResult Delete(int id)
        {
            var nv = _context.Set<NhanVien>().Find(id);

            if (nv == null)
                return NotFound();

            _context.Set<NhanVien>().Remove(nv);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}