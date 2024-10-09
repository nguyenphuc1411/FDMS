using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Data.Models
{
    public class Type
    {
        [Key]
        public int TypeID {  get; set; }
        public string TypeName { get; set; }
        public string? Note { get; set; }
        public DateTime Created_At { get; set; }= DateTime.Now;

        // Foreign Property
        public int UserID {  get; set; }

        // Foreign Key Link
        public User User { get; set; }

        public ICollection<Document> Documents { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
