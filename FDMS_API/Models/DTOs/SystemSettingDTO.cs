
using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs
{
    public class SystemSettingDTO
    {
        [Required]
        [Range(0, 2, ErrorMessage = "Theme must be between 0 to 2")]
        public int Theme { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
        [Required]
        public bool IsCaptchaRequired { get; set; }
    }
}
