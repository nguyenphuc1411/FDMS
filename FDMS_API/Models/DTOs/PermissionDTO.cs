using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs
{
    public class PermissionDTO
    {
        public bool CanRead { get; set; } = false;
        public bool CanModify { get; set; } = false;
        public int TypeID { get; set; }
        [Required]
        public int GroupID { get; set; }
    }
}
