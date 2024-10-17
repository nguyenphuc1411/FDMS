namespace FDMS_API.Models.DTOs
{
    public class GetGroup
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string? Note { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
