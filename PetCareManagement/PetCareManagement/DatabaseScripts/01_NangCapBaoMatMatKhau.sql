USE QuanLyThuCung;
GO

-- 1. Thêm các cột bảo mật cho bảng TaiKhoan nếu chưa tồn tại
IF COL_LENGTH('dbo.TaiKhoan', 'PasswordHash') IS NULL
BEGIN
    ALTER TABLE dbo.TaiKhoan ADD PasswordHash NVARCHAR(200) NULL;
END
GO

IF COL_LENGTH('dbo.TaiKhoan', 'PasswordSalt') IS NULL
BEGIN
    ALTER TABLE dbo.TaiKhoan ADD PasswordSalt NVARCHAR(200) NULL;
END
GO

IF COL_LENGTH('dbo.TaiKhoan', 'SoLanDangNhapSai') IS NULL
BEGIN
    ALTER TABLE dbo.TaiKhoan ADD SoLanDangNhapSai INT NOT NULL CONSTRAINT DF_TaiKhoan_SoLanDangNhapSai DEFAULT 0;
END
GO

IF COL_LENGTH('dbo.TaiKhoan', 'ThoiGianKhoa') IS NULL
BEGIN
    ALTER TABLE dbo.TaiKhoan ADD ThoiGianKhoa DATETIME NULL;
END
GO

IF COL_LENGTH('dbo.TaiKhoan', 'NgayDoiMatKhau') IS NULL
BEGIN
    ALTER TABLE dbo.TaiKhoan ADD NgayDoiMatKhau DATETIME NULL;
END
GO

-- 2. Tạo bảng nhật ký đăng nhập nếu chưa tồn tại
IF OBJECT_ID('dbo.NhatKyDangNhap', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.NhatKyDangNhap
    (
        NhatKyId INT IDENTITY(1,1) PRIMARY KEY,
        TaiKhoanId INT NULL,
        TenDangNhap NVARCHAR(50) NULL,
        TrangThai BIT NOT NULL,
        NoiDung NVARCHAR(255) NULL,
        DiaChiIp NVARCHAR(50) NULL,
        ThoiGian DATETIME NOT NULL CONSTRAINT DF_NhatKyDangNhap_ThoiGian DEFAULT GETDATE()
    );
END
GO

-- 3. Kiểm tra lại cấu trúc bảng sau khi nâng cấp
SELECT TOP 20
    TaiKhoanId,
    TenDangNhap,
    MatKhau,
    PasswordHash,
    PasswordSalt,
    SoLanDangNhapSai,
    ThoiGianKhoa,
    NgayDoiMatKhau,
    VaiTro
FROM dbo.TaiKhoan;
GO
