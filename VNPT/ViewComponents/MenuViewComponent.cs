using Microsoft.AspNetCore.Mvc;
using VNPT.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace VNPT.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly TestDBContext _db;
        public MenuViewComponent(TestDBContext db) => _db = db;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy toàn bộ menu từ DB
            var menus = await _db.MENU.OrderBy(m => m.ID).ToListAsync();
            return View(menus);
        }
    }
}
