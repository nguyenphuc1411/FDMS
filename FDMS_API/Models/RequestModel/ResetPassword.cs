using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.RequestModel
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@vietjetair\.com$", ErrorMessage = "Email must be @vietjetair.com domain.")]
        public string Email { get; set; }
        public string Token { get; set; }

        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{6,}", ErrorMessage = "Password invalid")]
        public string NewPassword { get; set; }
    }
}
