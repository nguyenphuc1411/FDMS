namespace FDMS_API.Models.DTOs.Document
{
    public class UserUploadDocument
    {
        public IFormFile File { get; set; }
        public int TypeID { get; set; }
        public int FlightID { get; set; }
    }
}
