using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Data.Models
{
    public class UserToken
    {
        [Key]
        public int ID { get; set; }
        public string Token { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string TokenType { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Email { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddMinutes(5);

        public bool IsUsed { get; set; } = false;

        public int UserID { get; set; }

        public User User { get; set; }
    }
}
