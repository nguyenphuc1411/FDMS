using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs.Type
{
    public class TypeDTO
    {
        [Required]
        [StringLength(255)]
        public string TypeName { get; set; }
        public string? Note { get; set; }
        [Required]
        public List<PermissionDTO> Permissions { get; set; }
    }
}
