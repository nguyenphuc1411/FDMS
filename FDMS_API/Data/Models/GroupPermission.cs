using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Data.Models
{
    public class GroupPermission
    {
        [Key]
        public int GroupID { get; set; }
        [Column(TypeName ="varchar(255)")]
        public string GroupName { get; set; }
        public string? Note { get; set; }
        public DateTime Created_At {  get; set; }= DateTime.Now;

        // Foreign Key Link

        public User User { get; set; }
    }
}
