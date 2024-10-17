namespace FDMS_API.Models.DTOs
{
    public class GetFlight
    {
        public int FlightID { get; set; }
        public string FlightNo { get; set; }
        public string Route { get; set; }
        public DateOnly DepartureDate { get; set; }
        public int TotalDocument {  get; set; }
        public bool IsFinished { get; set; }
    }
}
