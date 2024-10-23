using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs.Document
{
    public class VersionDTO
    {
        public IFormFile File { get; set; }
        public int DocumentID { get; set; }
    }
}
