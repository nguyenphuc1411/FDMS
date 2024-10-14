namespace FDMS_API.Models.ResponseModel
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}
