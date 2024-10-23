using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs.Document
{
    public class AdminUploadDocument
    {
        public string? Note { get; set; }
        public IFormFile File { get; set; }
        public int TypeID { get; set; }
        public int FlightID { get; set; }
        public List<int> GroupIDs { get; set; } // các quyền truy cập vào tài liệu
    }
}
