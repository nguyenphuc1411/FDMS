using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs.User
{
    public class UserDTO
    {
        [StringLength(255)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@vietjetair\.com$", ErrorMessage = "Email must be @vietjetair.com domain.")]
        public string Email { get; set; }
        [StringLength(11)]
        public string Phone {  get; set; }
        public List<int> Groups { get; set; }
    }
}
