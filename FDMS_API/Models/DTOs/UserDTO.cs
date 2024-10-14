using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Models.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsTerminated { get; set; }
        public string Role { get; set; }
    }
}
