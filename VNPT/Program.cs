using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using VNPT;
using VNPT.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký JwtService vào hệ thống DI
builder.Services.AddScoped<VNPT.Service.JwtService>();

//configure jwt settings
var jwtSettings = new Jwtsettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

//configure jwt authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
}
);

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Đăng ký TestDBContext sử dụng SQLite
builder.Services.AddDbContext<TestDBContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session hết hạn sau 30p không hoạt động
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();//session middware

app.UseRouting();

app.UseAuthentication(); // ĐƯA LÊN TRƯỚC: Nhận diện Token trước
app.UseAuthorization();  // ĐƯA XUỐNG SAU: Kiểm tra quyền sau

//app.UseEndpoints(async EndPoint=>
//{
//    await context.Response.WriteAsync("Hello world");
//});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

// --- ĐOẠN CODE TỰ ĐỘNG TẠO TÀI KHOẢN ADMIN MẪU ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<VNPT.Models.Data.TestDBContext>();

    //// Kiểm tra xem bảng USERS đã có dữ liệu chưa
    //if (!context.USER.Any())
    //{
    //// Nếu chưa có, tự động thêm 1 tài khoản admin/123456
    //context.USER.Add(new VNPT.Models.USER
    //{
    //USERNAME = "admin",
    //PASSWORD = VNPT.Common.toMD5.GetMd5Hash("123"), // Tạm thời để pass thô, khi nào làm hàm băm MD5 thì sửa sau
    //FULLNAME = "Quản Trị Viên",
    //ISDELETED = 0
    //       });
    //context.SaveChanges(); // Lưu vào file sqlite
    //    }
    //}
    // ------------------------------------------------
    if (!context.MENU.Any())
    {
        context.MENU.AddRange(new List<VNPT.Models.MENU>
    {
        new VNPT.Models.MENU { MENU_NAME = "QL_USER", MENU_TITLE = "Quản lý người dùng", CONTROLLER_NAME = "User", ACTION_NAME = "Index", ICON_CLASS = "bi bi-people" },
        new VNPT.Models.MENU { MENU_NAME = "QL_MENU", MENU_TITLE = "Quản lý menu", CONTROLLER_NAME = "Menu", ACTION_NAME = "Index", ICON_CLASS = "bi bi-menu-button-wide" }
    });
        context.SaveChanges();
    }
}

app.Run();
