using System.ComponentModel.DataAnnotations;

namespace FDMS_API.Models.DTOs.Group
{
    public class GroupDTO
    {
        [Required]
        [StringLength(255)]
        public string GroupName { get; set; }
        public string? Note { get; set; }
        public List<int> UserIDs { get; set; }
    }
}
