using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Data.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(11)")]
        public string Phone { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string PasswordHash { get; set; }
        public bool IsTerminated { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Role { get; set; }

        // Foreign Key Link
        public ICollection<Flight> Flights { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Type> Types { get; set; }

        public ICollection<Report> Reports { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
