using Microsoft.EntityFrameworkCore;
using QuanLyChamSocThuCung.Data;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ MVC + Views
builder.Services.AddControllersWithViews();

// Thêm Session (bắt buộc)
builder.Services.AddDistributedMemoryCache(); // Backend lưu session (in-memory cho dev, production dùng Redis/SQL)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
    options.Cookie.HttpOnly = true;                 // Bảo mật
    options.Cookie.IsEssential = true;              // GDPR compliant
});

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware pipeline (thứ tự quan trọng!)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Phải dùng UseSession() TRƯỚC UseRouting() và UseAuthorization()
app.UseSession();  // <-- Thêm dòng này ở đây!

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();