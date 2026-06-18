using System.ComponentModel.DataAnnotations.Schema;

namespace VNPT.Models
{
    [Table("MENU")]
    public class MENU
    {
        public int ID { get; set; }
        public int? PARENT_ID { get; set; }
        public string? MENU_NAME { get; set; }
        public string? MENU_TITLE { get; set; }
        public string? CONTROLLER_NAME { get; set; }
        public string? ACTION_NAME { get; set; }
        public string? ROUTE_URL { get; set; }
        public string? ICON_CLASS { get; set; }
    }
}
