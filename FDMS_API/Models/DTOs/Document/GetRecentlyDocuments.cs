namespace FDMS_API.Models.DTOs.Document
{
    public class GetRecentlyDocuments
    {
        public int DocumentID { get; set; }
        public string Title { get; set; }
        public string DocumentType { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Creator { get; set; }
        public string FlightNo { get; set; }
    }
}
