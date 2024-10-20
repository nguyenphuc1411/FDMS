namespace FDMS_API.Models.DTOs.Group
{
    public class GetGroups
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string? Note { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalMembers { get; set; }
    }
}
