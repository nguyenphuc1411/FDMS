using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Data.Models
{
    public class Flight
    {
        [Key]
        public int FlightID { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string FlightNo { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string POL { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string POU { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string AircraftID { get; set; }

        // Foreign Key Link
        public User User { get; set; }
    }
}
