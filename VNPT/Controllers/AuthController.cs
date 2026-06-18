using Microsoft.AspNetCore.Mvc;
using VNPT.Models;
using VNPT.Models.Data;

namespace VNPT.Controllers
{
    public class AuthController : Controller
    {

        private readonly ILogger<AuthController> _logger;//DI

        private readonly TestDBContext _db;

        public AuthController(ILogger<AuthController> logger, TestDBContext db )
        {
            _logger = logger;
            _db = db;
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
        public ActionResult Login(USER u) // 'u' này chứa USERNAME và PASSWORD người dùng vừa gõ từ màn hình
        {
            // CHỖ XỬ LÝ LOGIC 1: Kiểm tra xem người dùng có bỏ trống ô nào không
            if (u == null || string.IsNullOrEmpty(u.USERNAME) || string.IsNullOrEmpty(u.PASSWORD))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ tài khoản và mật khẩu!";
                return View(); // Trả về lại trang đăng nhập kèm thông báo lỗi
            }

            // CHỖ XỬ LÝ LOGIC 2: Lấy dữ liệu người dùng nhập, "bắn" lệnh xuống Database để kiểm tra
            // Câu lệnh này dịch ra tiếng người là: "Tìm trong bảng USER xem có ông nào trùng cả USERNAME và PASSWORD không"
            string hashedInput = Common.toMD5.GetMd5Hash(u.PASSWORD);

            var obj = _db.USER.FirstOrDefault(x => x.USERNAME.Equals(u.USERNAME) && x.PASSWORD.Equals(hashedInput) && x.ISDELETED == 0);

            // CHỖ XỬ LÝ LOGIC 3: Kiểm tra kết quả từ Database trả về
            if (obj != null)
            {
                // Nếu tìm thấy (obj khác null) -> ĐÚNG USERNAME VÀ PASSWORD
                // Tiến hành cấp "vé thông hành" (Session) cho người dùng
                HttpContext.Session.SetString("USERNAME", obj.USERNAME);
                //HttpContext.Session.SetString("FULLNAME", obj.FULLNAME ?? obj.USERNAME);
                HttpContext.Session.SetString("FULLNAME", obj.FULLNAME);
                HttpContext.Session.SetString("ROLE", obj.ROLE ?? "USER");
                // Chuyển hướng họ vào trang chủ (Trang Home)
                return RedirectToAction("Index", "Home");
            }

            // CHỖ XỬ LÝ LOGIC 4: Nếu không tìm thấy (obj bị null) -> SAI TÀI KHOẢN HOẶC MẬT KHẨU
            ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
            return View(); // Giữ họ lại trang đăng nhập và báo lỗi lên màn hình Nice Admin
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
