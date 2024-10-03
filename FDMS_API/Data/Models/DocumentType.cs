using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Data.Models
{
    public class DocumentType
    {
        [Key]
        public int TypeID {  get; set; }
        public string TypeName { get; set; }
        public string? Note { get; set; }
        public DateTime Created_At { get; set; }= DateTime.Now;

        // Foreign Key Link

        public User User { get; set; }
    }
}
