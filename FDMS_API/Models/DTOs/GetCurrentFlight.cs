namespace FDMS_API.Models.DTOs
{
    public class GetCurrentFlight
    {
        public int FlightID { get; set; }
        public string FlightNo { get; set; }
        public DateOnly FlightDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public string AircraftID { get; set; }
        public int SendFiles { get; set; }
        public int ReturnFiles { get; set; }
    }
}
