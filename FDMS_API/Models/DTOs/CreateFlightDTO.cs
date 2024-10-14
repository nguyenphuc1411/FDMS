using FDMS_API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs
{
    public class CreateFlightDTO
    {
        public int? FlightID { get; set; }
        [Required]        
        [StringLength(20,MinimumLength =3)]
        public string FlightNo { get; set; }

        [Required]
        public DateOnly FlightDate { get; set; }

        public TimeOnly? DepartureTime { get; set; }

        public TimeOnly? ArrivalTime { get; set; }

        [Required]
        [StringLength(150)]
        public string POL { get; set; }

        [Required]
        [StringLength(150)]
        public string POU { get; set; }

        [StringLength(20)]
        public string? AircraftID { get; set; }

    }
}
