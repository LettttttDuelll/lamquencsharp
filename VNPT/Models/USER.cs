using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VNPT.Models
{
    [Table("USERS")]
    public class USER
    {
        [Key]
        public int ID { get; set; }
        public string? USERNAME { get; set; }
        public string? PASSWORD { get; set; }
        public string? FULLNAME {  get; set; }
        public int ISDELETED { get; set; }
        public string? ROLE { get; set; }
    }
}
