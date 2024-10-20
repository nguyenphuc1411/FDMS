using FDMS_API.Models.DTOs.User;

namespace FDMS_API.Models.DTOs.Group
{
    public class GetGroup
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string? Note { get; set; }
        public List<GetUser> Users { get; set; }
    }
}
