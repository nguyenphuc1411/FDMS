using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.RequestModel
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@vietjetair\.com$", ErrorMessage = "Email must be @vietjetair.com domain.")]
        public string Email { get; set; }
    }
}
