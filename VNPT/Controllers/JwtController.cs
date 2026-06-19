using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VNPT.Common;
using VNPT.Models.Data;
using VNPT.Service;

namespace VNPT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly Jwtsettings _jwtsettings;
        private readonly TestDBContext _db;
        private readonly JwtService _jwtservice;
        public JwtController(Jwtsettings jwtsettings, TestDBContext db,JwtService jwtService) 
        { 
            _jwtsettings = jwtsettings; 
            _db = db;
            _jwtservice = jwtService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login (string username, string password)
        {
            var hassedPass = toMD5.GetMd5Hash(password);
            var u = _db.USER.FirstOrDefault(u=>u.USERNAME == username&&u.PASSWORD==hassedPass&&u.ISDELETED==0);
            if (u == null)
            {
                return Ok(new GenericResult(false, "Tài khoản hoặc mật khẩu không chính xác"));
            }
            var token = _jwtservice.GenerateToken(username, u.ROLE);
            return Ok(new GenericResult(true, "Đăng nhập thành công",token));
        }
    }
}
