using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VNPT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Authorize]//Nhờ có bộ lọc [Authorize] đặt trên đầu hàm Get(), căn phòng này được khóa chặt.
        public IActionResult Get()
        {
            return Ok(new {value = "This is a protected value"});
        }
    }
}
