
using System.ComponentModel.DataAnnotations;

namespace FDMS_API.DTOs
{
    public class SystemSettingDTO
    {
        [Range(0,2,ErrorMessage ="Theme must be between 0 to 2")]
        public int Theme { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool IsCaptchaRequired { get; set; }
    }
}
