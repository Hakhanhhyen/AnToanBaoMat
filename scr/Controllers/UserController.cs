using Microsoft.AspNetCore.Mvc;
using QuanLyChamSocThuCung.Data;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dichvu = _context.DichVus.ToList();
            return View(dichvu);
        }
    }
}