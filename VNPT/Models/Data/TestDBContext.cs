using Microsoft.EntityFrameworkCore;
using VNPT.Models; // Trỏ đến thư mục chứa class USER của bạn

namespace VNPT.Models.Data
{
    public class TestDBContext : DbContext
    {
        public TestDBContext(DbContextOptions<TestDBContext> options) : base(options)
        {
        }

        // Khai báo các bảng sẽ có trong Database
        // Tên thuộc tính (USER) sẽ là tên bảng, kiểu dữ liệu <USER> là class model
        public DbSet<USER> USER { get; set; }
        // Sau này có bảng MENU thì bạn thêm: public DbSet<MENU> MENU { get; set; } vào đây
        public DbSet<MENU> MENU { get; set; }
    }
}
