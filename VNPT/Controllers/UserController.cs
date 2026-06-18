using Microsoft.AspNetCore.Mvc;
using VNPT.Models;
using VNPT.Models.Data;
using VNPT.Common;

namespace VNPT.Controllers
{
    [Route("quan-ly-user")]
    public class UserController : Controller
    {
        private readonly TestDBContext _db;

        public UserController(TestDBContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("USERNAME") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                //return View();
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet("danh-sach")]
        public IActionResult GetAll() {
            if (HttpContext.Session.GetString("USERNAME") == null)
            {
                return Ok(new GenericResult { Success = false, Message = "Hết phiên đăng nhập. Vui lòng đăng nhập lại!" });
            }
            try
            {
                var users = _db.USER
                    .Where(x => x.ISDELETED == 0)
                    .Select(x => new { x.ID, x.USERNAME, x.FULLNAME })
                    .ToList();

                return Ok(new GenericResult { Success = true, Data = users });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResult { Success = false, Message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [HttpPost("luu")]
        public IActionResult SaveData([FromForm] USER u)
        {
            if (HttpContext.Session.GetString("USERNAME") == null)
            {
                return Ok(new GenericResult { Success = false, Message = "Bạn không có quyền thực hiện thao tác này!" });
            }

            try
            {
                // Validate dữ liệu cơ bản ở Backend để tránh dữ liệu rác
                if (string.IsNullOrEmpty(u.USERNAME))
                    return Ok(new GenericResult { Success = false, Message = "Tài khoản không được để trống." });

                if (u.ID > 0)
                {
                    var existUser = _db.USER.FirstOrDefault(x => x.ID == u.ID && x.ISDELETED == 0);
                    if (existUser == null)
                        return Ok(new GenericResult { Success = false, Message = "Tài khoản không tồn tại hoặc đã bị xóa!" });

                    existUser.FULLNAME = u.FULLNAME;

                    if (!string.IsNullOrEmpty(u.PASSWORD))
                    {
                        existUser.PASSWORD = Common.toMD5.GetMd5Hash(u.PASSWORD);
                    }
                }
                // TRƯỜNG HỢP: THÊM MỚI USER
                else
                {
                    if (string.IsNullOrEmpty(u.PASSWORD))
                        return Ok(new GenericResult { Success = false, Message = "Mật khẩu không được để trống." });

                    // Kiểm tra trùng lặp Username trong Database
                    var isExist = _db.USER.Any(x => x.USERNAME.Equals(u.USERNAME));
                    if (isExist)
                        return Ok(new GenericResult { Success = false, Message = "Tên tài khoản này đã tồn tại trên hệ thống!" });

                    // Tiến hành băm mật khẩu bằng hàm MD5 chính chủ của bạn
                    u.PASSWORD = Common.toMD5.GetMd5Hash(u.PASSWORD);
                    u.ISDELETED = 0; // Đảm bảo trạng thái hoạt động
                    u.ROLE = "USER";
                    _db.USER.Add(u);
                }

                _db.SaveChanges(); // Lưu cập nhật vào file SQLite
                return Ok(new GenericResult { Success = true, Message = "Lưu thông tin tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResult { Success = false, Message = "Lỗi khi lưu dữ liệu: " + ex.Message });
            }
        }

        // =========================================================================
        // 4. API XÓA MỀM USER (Bảo mật bằng Session trực tiếp)
        // =========================================================================
        [HttpPost("xoa")]
        public IActionResult DeleteData([FromForm] int id)
        {
            // LỚP BẢO MẬT API: Kiểm tra quyền đăng nhập trước khi xóa dữ liệu
            if (HttpContext.Session.GetString("USERNAME") == null)
            {
                return Ok(new GenericResult { Success = false, Message = "Bạn không có quyền thực hiện thao tác này!" });
            }
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role.ToLower() != "admin")
            {
                return Json(new { success = false, message = "Bạn không có quyền thực hiện chức năng này!" });
            }
            try
            {
                var user = _db.USER.FirstOrDefault(x => x.ID == id);
                if (user == null)
                    return Ok(new GenericResult { Success = false, Message = "Không tìm thấy tài khoản cần xóa." });

                // Không cho phép tài khoản đang đăng nhập tự xóa chính mình
                var currentSessionUser = HttpContext.Session.GetString("USERNAME");
                if (user.USERNAME.Equals(currentSessionUser))
                    return Ok(new GenericResult { Success = false, Message = "Bạn không thể tự xóa tài khoản chính mình đang dùng!" });

                // Thực hiện xóa mềm (Update flag ISDELETED = 1) giống quy trình của công ty
                user.ISDELETED = 1;

                _db.SaveChanges();
                return Ok(new GenericResult { Success = true, Message = "Đã xóa tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResult { Success = false, Message = "Lỗi khi xóa tài khoản: " + ex.Message });
            }
        }
    }

}

