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

        // Foreign property
        public int UserID {  get; set; }

        // Foreign Key Link

        public User User { get; set; }
        
        public ICollection<Type_GroupPermission> Type_GroupPermissions { get; set; }
        public ICollection<User_Group> User_Groups { get; set; }
        public ICollection<DocumentPermission> DocumentPermissions { get; set; }
    }
}
