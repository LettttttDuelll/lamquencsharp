using Microsoft.AspNetCore.Mvc;
using VNPT.Models;
using VNPT.Models.Data;
using VNPT.Service;

namespace VNPT.Controllers
{
    public class AuthController : Controller
    {

        private readonly ILogger<AuthController> _logger;//DI
        private readonly TestDBContext _db;
        private readonly JwtService _jwtService;

        public AuthController(ILogger<AuthController> logger, TestDBContext db , JwtService jwtService)
        {
            _logger = logger;
            _db = db;
            _jwtService = jwtService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("USERNAME") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("USERNAME") == null)
            {
                return View();//trả về trang Auth/Login(controller/action)
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
        [HttpPost]
        public ActionResult Login(USER u)
        {
            if (u == null || string.IsNullOrEmpty(u.USERNAME) || string.IsNullOrEmpty(u.PASSWORD))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ tài khoản và mật khẩu!";
                return View();
            }
            string hashedInput = Common.toMD5.GetMd5Hash(u.PASSWORD);

            var obj = _db.USER.FirstOrDefault(x => x.USERNAME.Equals(u.USERNAME) && x.PASSWORD.Equals(hashedInput) && x.ISDELETED == 0);

            if (obj != null)
            {
                HttpContext.Session.SetString("USERNAME", obj.USERNAME);
                HttpContext.Session.SetString("FULLNAME", obj.FULLNAME);
                HttpContext.Session.SetString("ROLE", obj.ROLE ?? "USER");
                // Chuyển hướng họ vào trang chủ (Trang Home)
                var token = _jwtService.GenerateToken(obj.USERNAME, obj.ROLE);

                // Gói token vào cookie tên là "jwt_token", lưu trong vòng 30 phút
                Response.Cookies.Append("jwt_token", token, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    HttpOnly = false // Để false thì JavaScript ở giao diện mới đọc được tấm vé này
                });
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Index");
        }

    }
}
