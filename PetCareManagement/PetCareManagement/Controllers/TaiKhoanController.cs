using Microsoft.AspNetCore.Mvc;
using QuanLyChamSocThuCung.Data;
using QuanLyChamSocThuCung.Models;
using QuanLyChamSocThuCung.Services;
using System.Linq;

namespace QuanLyChamSocThuCung.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Sai 5 lần thì khóa tài khoản 5 phút.
        private const int SoLanSaiToiDa = 5;
        private const int SoPhutKhoaTaiKhoan = 5;

        public TaiKhoanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string TenDangNhap, string MatKhau)
        {
            TenDangNhap = TenDangNhap?.Trim() ?? string.Empty;
            MatKhau = MatKhau ?? string.Empty;

            if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau))
            {
                ViewBag.Error = "Vui lòng nhập tên đăng nhập và mật khẩu!";
                return View();
            }

            var user = _context.TaiKhoans.FirstOrDefault(u => u.TenDangNhap == TenDangNhap);

            if (user == null)
            {
                GhiNhatKyDangNhap(null, TenDangNhap, false, "Đăng nhập thất bại: tài khoản không tồn tại");
                _context.SaveChanges();

                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }

            if (user.ThoiGianKhoa.HasValue && user.ThoiGianKhoa.Value > DateTime.Now)
            {
                var soPhutConLai = Math.Ceiling((user.ThoiGianKhoa.Value - DateTime.Now).TotalMinutes);

                GhiNhatKyDangNhap(user.TaiKhoanId, user.TenDangNhap, false,
                    $"Tài khoản đang bị khóa. Còn khoảng {soPhutConLai} phút");
                _context.SaveChanges();

                ViewBag.Error = $"Tài khoản đang bị khóa tạm thời. Vui lòng thử lại sau khoảng {soPhutConLai} phút.";
                return View();
            }

            if (user.ThoiGianKhoa.HasValue && user.ThoiGianKhoa.Value <= DateTime.Now)
            {
                user.ThoiGianKhoa = null;
                user.SoLanDangNhapSai = 0;
                _context.TaiKhoans.Update(user);
                _context.SaveChanges();
            }

            bool matKhauDung = KiemTraMatKhau(user, MatKhau);

            if (matKhauDung)
            {
                user.SoLanDangNhapSai = 0;
                user.ThoiGianKhoa = null;

                HttpContext.Session.SetInt32("TaiKhoanId", user.TaiKhoanId);
                HttpContext.Session.SetString("Role", user.VaiTro);
                HttpContext.Session.SetString("TenDangNhap", user.TenDangNhap);

                GhiNhatKyDangNhap(user.TaiKhoanId, user.TenDangNhap, true, "Đăng nhập thành công");
                _context.TaiKhoans.Update(user);
                _context.SaveChanges();

                if (user.VaiTro == "Admin" || user.VaiTro == "NhanVien" || user.VaiTro == "KhachHang")
                {
                    return RedirectToAction("Dashboard", "QuanTri");
                }

                return RedirectToAction("Index", "Home");
            }

            user.SoLanDangNhapSai += 1;

            if (user.SoLanDangNhapSai >= SoLanSaiToiDa)
            {
                user.SoLanDangNhapSai = SoLanSaiToiDa;
                user.ThoiGianKhoa = DateTime.Now.AddMinutes(SoPhutKhoaTaiKhoan);

                string noiDungKhoa = $"Sai mật khẩu lần {SoLanSaiToiDa}. Tài khoản bị khóa {SoPhutKhoaTaiKhoan} phút";

                GhiNhatKyDangNhap(user.TaiKhoanId, user.TenDangNhap, false, noiDungKhoa);
                _context.TaiKhoans.Update(user);
                _context.SaveChanges();

                ViewBag.Error = $"Sai mật khẩu lần {SoLanSaiToiDa}. Tài khoản đã bị khóa {SoPhutKhoaTaiKhoan} phút.";
                return View();
            }

            int soLanConLai = SoLanSaiToiDa - user.SoLanDangNhapSai;
            string noiDungSai = $"Sai mật khẩu lần {user.SoLanDangNhapSai}. Còn {soLanConLai} lần thử";

            GhiNhatKyDangNhap(user.TaiKhoanId, user.TenDangNhap, false, noiDungSai);
            _context.TaiKhoans.Update(user);
            _context.SaveChanges();

            ViewBag.Error = $"Sai mật khẩu lần {user.SoLanDangNhapSai}. Bạn còn {soLanConLai} lần thử.";
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string TenDangNhap, string MatKhau, string DienThoai, string VaiTro)
        {
            TenDangNhap = TenDangNhap?.Trim() ?? string.Empty;
            MatKhau = MatKhau ?? string.Empty;

            if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau) ||
                string.IsNullOrWhiteSpace(DienThoai) || string.IsNullOrWhiteSpace(VaiTro))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            if (!KiemTraDoManhMatKhau(MatKhau, TenDangNhap, out string loiMatKhau))
            {
                ViewBag.Error = loiMatKhau;
                return View();
            }

            if (_context.TaiKhoans.Any(u => u.TenDangNhap == TenDangNhap))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            PasswordHasher.CreatePasswordHash(MatKhau, out string passwordHash, out string passwordSalt);

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = TenDangNhap,
                MatKhau = "Đã bảo vệ",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                SoLanDangNhapSai = 0,
                ThoiGianKhoa = null,
                NgayDoiMatKhau = DateTime.Now,
                VaiTro = VaiTro
            };

            _context.TaiKhoans.Add(taiKhoan);
            _context.SaveChanges();

            if (VaiTro == "KhachHang")
            {
                var khachHang = new KhachHang
                {
                    HoTen = TenDangNhap,
                    DienThoai = DienThoai,
                    Email = TenDangNhap
                };
                _context.KhachHangs.Add(khachHang);
                _context.SaveChanges();
            }

            HttpContext.Session.SetInt32("TaiKhoanId", taiKhoan.TaiKhoanId);
            HttpContext.Session.SetString("Role", taiKhoan.VaiTro);
            HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetInt32("TaiKhoanId") == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string MatKhauCu, string MatKhauMoi, string XacNhanMatKhauMoi)
        {
            var taiKhoanId = HttpContext.Session.GetInt32("TaiKhoanId");

            if (taiKhoanId == null)
            {
                return RedirectToAction("Login");
            }

            if (string.IsNullOrWhiteSpace(MatKhauCu) || string.IsNullOrWhiteSpace(MatKhauMoi) || string.IsNullOrWhiteSpace(XacNhanMatKhauMoi))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            if (!KiemTraDoManhMatKhau(MatKhauMoi, HttpContext.Session.GetString("TenDangNhap"), out string loiMatKhau))
            {
                ViewBag.Error = loiMatKhau;
                return View();
            }

            if (MatKhauMoi != XacNhanMatKhauMoi)
            {
                ViewBag.Error = "Xác nhận mật khẩu mới không khớp!";
                return View();
            }

            var user = _context.TaiKhoans.FirstOrDefault(u => u.TaiKhoanId == taiKhoanId.Value);

            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            if (!KiemTraMatKhau(user, MatKhauCu))
            {
                ViewBag.Error = "Mật khẩu cũ không đúng!";
                return View();
            }

            PasswordHasher.CreatePasswordHash(MatKhauMoi, out string passwordHash, out string passwordSalt);

            user.MatKhau = "Đã bảo vệ";
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.SoLanDangNhapSai = 0;
            user.ThoiGianKhoa = null;
            user.NgayDoiMatKhau = DateTime.Now;

            _context.TaiKhoans.Update(user);
            _context.SaveChanges();

            ViewBag.Success = "Đổi mật khẩu thành công. Mật khẩu mới đã được bảo vệ bằng PBKDF2 + salt.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private bool KiemTraDoManhMatKhau(string matKhau, string? tenDangNhap, out string thongBaoLoi)
        {
            const int doDaiToiThieu = 8;

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                thongBaoLoi = "Mật khẩu không được để trống.";
                return false;
            }

            if (matKhau.Length < doDaiToiThieu)
            {
                thongBaoLoi = $"Mật khẩu phải có ít nhất {doDaiToiThieu} ký tự.";
                return false;
            }

            if (!matKhau.Any(char.IsUpper))
            {
                thongBaoLoi = "Mật khẩu phải có ít nhất 1 chữ hoa, ví dụ: A, B, C.";
                return false;
            }

            if (!matKhau.Any(char.IsLower))
            {
                thongBaoLoi = "Mật khẩu phải có ít nhất 1 chữ thường, ví dụ: a, b, c.";
                return false;
            }

            if (!matKhau.Any(char.IsDigit))
            {
                thongBaoLoi = "Mật khẩu phải có ít nhất 1 chữ số, ví dụ: 0, 1, 2.";
                return false;
            }

            if (!matKhau.Any(kyTu => !char.IsLetterOrDigit(kyTu)))
            {
                thongBaoLoi = "Mật khẩu phải có ít nhất 1 ký tự đặc biệt, ví dụ: @, #, $, !.";
                return false;
            }

            string[] matKhauYeuThuongGap =
            {
                "123456", "12345678", "123456789", "password", "admin",
                "admin123", "abc123", "qwerty", "111111", "matkhau", "matkhau123"
            };

            if (matKhauYeuThuongGap.Any(mk => string.Equals(mk, matKhau, StringComparison.OrdinalIgnoreCase)))
            {
                thongBaoLoi = "Mật khẩu quá phổ biến và dễ đoán. Vui lòng chọn mật khẩu mạnh hơn.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(tenDangNhap) &&
                matKhau.Contains(tenDangNhap, StringComparison.OrdinalIgnoreCase))
            {
                thongBaoLoi = "Mật khẩu không nên chứa tên đăng nhập.";
                return false;
            }

            thongBaoLoi = string.Empty;
            return true;
        }

        private bool KiemTraMatKhau(TaiKhoan user, string matKhauNhap)
        {
            if (!string.IsNullOrWhiteSpace(user.PasswordHash) && !string.IsNullOrWhiteSpace(user.PasswordSalt))
            {
                return PasswordHasher.VerifyPassword(matKhauNhap, user.PasswordHash, user.PasswordSalt);
            }

            // Dữ liệu cũ: còn lưu mật khẩu rõ. Cho đăng nhập đúng một lần rồi tự nâng cấp sang hash + salt.
            if (!string.IsNullOrWhiteSpace(user.MatKhau) && user.MatKhau == matKhauNhap)
            {
                PasswordHasher.CreatePasswordHash(matKhauNhap, out string passwordHash, out string passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.MatKhau = "Đã bảo vệ";
                user.NgayDoiMatKhau = DateTime.Now;
                return true;
            }

            return false;
        }

        private void GhiNhatKyDangNhap(int? taiKhoanId, string? tenDangNhap, bool trangThai, string noiDung)
        {
            var log = new NhatKyDangNhap
            {
                TaiKhoanId = taiKhoanId,
                TenDangNhap = tenDangNhap,
                TrangThai = trangThai,
                NoiDung = noiDung,
                DiaChiIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                ThoiGian = DateTime.Now
            };

            _context.NhatKyDangNhaps.Add(log);
        }
    }
}
