using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Data.Models
{
    public class VersionDocument
    {
        [Key]
        public int VersionID { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Title { get; set; }
        [Column(TypeName = "decimal(2,1)")]
        public decimal Version { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;

        public int UserID { get; set; }
        public User User { get; set; }
        public int DocumentID {  get; set; }
        public Document Document { get; set; }
    }
}
