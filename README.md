# Modern Password Authentication System

## Giới thiệu

Modern Password Authentication System là hệ thống xác thực người dùng được xây dựng bằng **ASP.NET Core MVC** kết hợp với **SQL Server**, nhằm bảo vệ mật khẩu người dùng trong cơ sở dữ liệu theo các phương pháp bảo mật hiện đại.

Hệ thống sử dụng **PBKDF2 kết hợp PasswordSalt** để băm mật khẩu trước khi lưu vào cơ sở dữ liệu, giúp tăng cường khả năng chống lại các cuộc tấn công Brute-force và Rainbow Table. Ngoài ra, hệ thống còn hỗ trợ khóa tài khoản sau nhiều lần đăng nhập sai, ghi nhật ký đăng nhập và đổi mật khẩu an toàn.

---

# Thành viên thực hiện

- Hà Khánh Huyền
- Đặng Kiều Loan

**Môn học:** Nhập môn An toàn Bảo mật Thông tin

---

# Công nghệ sử dụng

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- SQL Server Management Studio (SSMS)
- PBKDF2 (Password-Based Key Derivation Function 2)

---

# Chức năng của hệ thống

## 1. Đăng ký tài khoản

- Đăng ký người dùng mới.
- Kiểm tra tên đăng nhập đã tồn tại.
- Kiểm tra độ mạnh của mật khẩu.
- Sinh PasswordHash và PasswordSalt trước khi lưu vào cơ sở dữ liệu.

## 2. Đăng nhập

- Xác thực người dùng bằng PBKDF2 kết hợp PasswordSalt.
- So sánh PasswordHash với dữ liệu đã lưu trong cơ sở dữ liệu.
- Ghi nhật ký đăng nhập thành công hoặc thất bại.

## 3. Đổi mật khẩu

- Kiểm tra mật khẩu hiện tại.
- Sinh PasswordSalt mới.
- Sinh PasswordHash mới.
- Cập nhật dữ liệu trong cơ sở dữ liệu.

## 4. Khóa tài khoản

- Theo dõi số lần đăng nhập sai.
- Khóa tài khoản sau nhiều lần đăng nhập thất bại.

## 5. Nhật ký đăng nhập

Lưu các thông tin:

- Username
- Thời gian đăng nhập
- Địa chỉ IP (nếu có)
- Trạng thái đăng nhập (Thành công/Thất bại)

---

# Cấu trúc thư mục dự án

```text
ModernPasswordAuthenticationSystem
│
├── Controllers/          Xử lý yêu cầu từ người dùng
├── Models/               Các lớp mô hình dữ liệu
├── Views/                Giao diện người dùng
├── Data/                 DbContext và kết nối cơ sở dữ liệu
├── Services/             Các dịch vụ xử lý nghiệp vụ
├── wwwroot/              CSS, JavaScript, hình ảnh
├── appsettings.json      Chuỗi kết nối Database
├── Program.cs            Điểm khởi động chương trình
└── ModernPasswordAuthenticationSystem.sln
```

---

# Cấu trúc cơ sở dữ liệu

## Bảng TaiKhoan

| Trường | Mô tả |
|---------|------|
| Username | Tên đăng nhập |
| PasswordHash | Mật khẩu sau khi băm |
| PasswordSalt | Salt của từng tài khoản |
| FailedLoginAttempts | Số lần đăng nhập sai |
| LockoutEnd | Thời gian khóa tài khoản |

---

## Bảng NhatKyDangNhap

| Trường | Mô tả |
|---------|------|
| Username | Tên đăng nhập |
| LoginTime | Thời gian đăng nhập |
| IPAddress | Địa chỉ IP |
| Status | Thành công hoặc thất bại |

---

# Hướng dẫn cài đặt

## 1. Yêu cầu hệ thống

Trước khi chạy chương trình, cần cài đặt:

- Visual Studio 2022
- .NET SDK (đúng phiên bản dự án sử dụng)
- SQL Server 2019 hoặc SQL Server 2022
- SQL Server Management Studio (SSMS)
- Git (nếu tải source từ GitHub)

---

## 2. Tải mã nguồn

Clone dự án từ GitHub:

```bash
git clone https://github.com/your-repository.git
```

Hoặc tải trực tiếp file ZIP và giải nén.

---

## 3. Mở dự án

- Mở **Visual Studio 2022**
- Chọn **Open a project or solution**
- Mở file:

```
ModernPasswordAuthenticationSystem.sln
```

---

## 4. Khôi phục thư viện

Visual Studio sẽ tự động tải các package từ NuGet.

Nếu chưa tải tự động:

```
Tools
→ NuGet Package Manager
→ Restore NuGet Packages
```

Hoặc sử dụng Terminal:

```bash
dotnet restore
```

---

## 5. Cấu hình cơ sở dữ liệu

Mở file:

```
appsettings.json
```

Tìm phần:

```json
"ConnectionStrings": {
  "DefaultConnection": ""
}
```

Thay đổi thành:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ModernPasswordDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Nếu sử dụng SQL Authentication:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ModernPasswordDB;User Id=sa;Password=yourpassword;TrustServerCertificate=True;"
}
```

---

## 6. Khôi phục cơ sở dữ liệu

Có hai cách:

### Cách 1: Restore Database

- Mở SQL Server Management Studio.
- Chọn **Restore Database**.
- Chọn file:

```
ModernPasswordDB.bak
```

hoặc

```
ModernPasswordDB.sql
```

- Thực hiện Restore.

### Cách 2: Sử dụng Migration

Nếu dự án sử dụng Entity Framework Core:

Mở **Package Manager Console** và chạy:

```powershell
Update-Database
```

Hệ thống sẽ tự tạo cơ sở dữ liệu.

---

## 7. Chạy chương trình

Trong Visual Studio:

```
Build
→ Build Solution
```

Nếu Build thành công:

Nhấn:

```
F5
```

hoặc

```
Ctrl + F5
```

Website sẽ tự động mở trên trình duyệt.

---

# Hướng dẫn sử dụng

## Bước 1. Đăng ký tài khoản

- Chọn chức năng **Đăng ký**.
- Nhập:
  - Tên đăng nhập
  - Mật khẩu
  - Xác nhận mật khẩu

Hệ thống sẽ:

- Kiểm tra Username đã tồn tại hay chưa.
- Kiểm tra độ mạnh của mật khẩu.
- - Sinh PasswordSalt ngẫu nhiên.
- Sinh PasswordHash bằng PBKDF2.
- Lưu dữ liệu vào bảng **TaiKhoan**.

---

## Bước 2. Đăng nhập

- Chọn chức năng **Đăng nhập**.
- Nhập Username và Password.

Hệ thống sẽ:

- Lấy PasswordSalt từ cơ sở dữ liệu.
- Sinh lại PasswordHash bằng PBKDF2.
- So sánh với PasswordHash đã lưu.

Nếu đúng:

- Đăng nhập thành công.
- Ghi nhật ký đăng nhập.

Nếu sai:

- Thông báo sai mật khẩu.
- Tăng số lần đăng nhập sai.
- Ghi nhật ký đăng nhập.

---

## Bước 3. Kiểm tra khóa tài khoản

- Nhập sai mật khẩu liên tiếp nhiều lần (theo cấu hình hệ thống, ví dụ: 5 lần).

Kết quả:

- Tài khoản bị khóa.
- Người dùng không thể đăng nhập cho đến khi hết thời gian khóa hoặc được mở khóa.

---

## Bước 4. Đổi mật khẩu

Sau khi đăng nhập:

- Chọn **Đổi mật khẩu**.
- Nhập:
  - Mật khẩu hiện tại
  - Mật khẩu mới
  - Xác nhận mật khẩu mới

Nếu hợp lệ:

- Sinh PasswordSalt mới.
- Sinh PasswordHash mới.
- Cập nhật dữ liệu trong cơ sở dữ liệu.

---

## Bước 5. Kiểm tra dữ liệu trong cơ sở dữ liệu

Mở SQL Server Management Studio.

### Kiểm tra bảng TaiKhoan

Đảm bảo:

- Không lưu mật khẩu dạng rõ.
- Có các cột:
  - PasswordHash
  - PasswordSalt

### Kiểm tra bảng NhatKyDangNhap

Sau mỗi lần đăng nhập sẽ lưu:

- Username
- Thời gian đăng nhập
- Địa chỉ IP
- Trạng thái đăng nhập (Thành công hoặc Thất bại)

---

# Kiểm thử hệ thống

| STT | Chức năng kiểm thử | Kết quả mong đợi |
|-----|--------------------|------------------|
| 1 | Đăng ký tài khoản mới | Thành công |
| 2 | Đăng ký với Username đã tồn tại | Hiển thị thông báo lỗi |
| 3 | Đăng nhập đúng | Thành công |
| 4 | Đăng nhập sai | Hiển thị thông báo lỗi |
| 5 | Đăng nhập sai nhiều lần | Tài khoản bị khóa |
| 6 | Đổi mật khẩu | Thành công |
| 7 | Đăng nhập bằng mật khẩu mới | Thành công |
| 8 | Kiểm tra PasswordHash và PasswordSalt | Không lưu mật khẩu gốc |
| 9 | Hai tài khoản cùng mật khẩu | PasswordSalt khác nhau |
| 10 | Kiểm tra nhật ký đăng nhập | Có bản ghi mới sau mỗi lần đăng nhập |

---

# Hướng phát triển

Trong tương lai, hệ thống có thể được mở rộng với các chức năng:

- Tích hợp xác thực hai yếu tố (2FA) bằng Email hoặc ứng dụng xác thực.
- Bổ sung CAPTCHA nhằm hạn chế các cuộc tấn công đăng nhập tự động.
- Hỗ trợ xác minh Email khi đăng ký tài khoản hoặc khôi phục mật khẩu.
- - Xây dựng cơ chế cảnh báo đăng nhập bất thường khi phát hiện truy cập từ địa chỉ IP hoặc thiết bị lạ.
- Triển khai hệ thống trên môi trường thực tế và tối ưu hiệu năng để phục vụ nhiều người dùng hơn.

---

# Kết quả đạt được

- Xây dựng hệ thống xác thực mật khẩu an toàn bằng PBKDF2 kết hợp PasswordSalt.
- Không lưu mật khẩu dưới dạng văn bản thuần (Plain Text).
- Mỗi tài khoản có PasswordSalt riêng nhằm tăng cường bảo mật.
- Hỗ trợ đăng ký, đăng nhập và đổi mật khẩu an toàn.
- Khóa tài khoản sau nhiều lần đăng nhập sai.
- Ghi nhật ký đăng nhập thành công và thất bại vào cơ sở dữ liệu.
- Đáp ứng đầy đủ các yêu cầu của đề tài về bảo vệ mật khẩu người dùng.

---

# Giấy phép

Dự án được phát triển phục vụ mục đích học tập trong học phần **Nhập môn An toàn Bảo mật Thông tin**.
