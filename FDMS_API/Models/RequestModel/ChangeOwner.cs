using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.RequestModel
{
    public class ChangeOwner
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@vietjetair\.com$", ErrorMessage = "Email must be @vietjetair.com domain.")]
        public string Email {  get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{6,}", ErrorMessage = "Password invalid")]
        public string ConfirmPassword { get; set; }
    }
}
