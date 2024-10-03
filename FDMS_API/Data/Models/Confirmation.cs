using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Data.Models
{
    public class Confirmation
    {
      
        [Column(TypeName ="varchar(255)")]
        public string SignatureURL { get; set; }
        public DateTime Confirmed_At { get; set; } = DateTime.Now;

        // Foreign Property
        public int UserID { get; set; }
        public int FlightID { get; set; }

        // Foreign Key Link
        public User User { get; set; }
        public Flight Flight { get; set; }
    }
}
