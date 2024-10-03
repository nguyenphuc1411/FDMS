using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Data.Models
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Name { get; set; }
        [Column(TypeName = "decimal(2,1)")]
        public decimal Version {  get; set; }
        public string? Note {  get; set; }
        [Column(TypeName = "varchar(255)")]
        public string FilePath {  get; set; }
        public DateTime Created_At { get; set; } = DateTime.Now;

        // Foreign Key Link
        public DocumentType DocumentType { get; set; }
        public User User { get; set; }
        public Flight Flight { get; set; }
    }
}
