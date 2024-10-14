using FDMS_API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs
{
    public class FlightDTO
    {
        public int FlightID { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string FlightNo { get; set; }

        public DateOnly FlightDate { get; set; }

        public TimeOnly DepartureTime { get; set; }

        public TimeOnly ArrivalTime { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string POL { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string POU { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string AircraftID { get; set; }

    }
}
