using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs
{
    public class PermissionDTO
    {
        public bool CanRead { get; set; } 
        public bool CanModify { get; set; }
        [Required]
        public int GroupID { get; set; }
    }
}
