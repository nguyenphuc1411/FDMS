using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS_API.Models.DTOs.User
{
    public class GetUser
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
