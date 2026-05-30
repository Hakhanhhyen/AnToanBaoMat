USE QuanLyThuCung;
GO

-- Kiểm tra mật khẩu không còn lưu dạng rõ đối với tài khoản đã đăng ký/nâng cấp
SELECT
    TaiKhoanId,
    TenDangNhap,
    MatKhau,
    PasswordHash,
    PasswordSalt,
    LEN(PasswordHash) AS DoDaiHash,
    LEN(PasswordSalt) AS DoDaiSalt,
    SoLanDangNhapSai,
    ThoiGianKhoa,
    NgayDoiMatKhau,
    VaiTro
FROM dbo.TaiKhoan;
GO

-- Kiểm tra log đăng nhập thành công/thất bại
SELECT TOP 50
    NhatKyId,
    TaiKhoanId,
    TenDangNhap,
    TrangThai,
    NoiDung,
    DiaChiIp,
    ThoiGian
FROM dbo.NhatKyDangNhap
ORDER BY NhatKyId DESC;
GO
