namespace FDMS_API.Models.DTOs
{
    public class GetType
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Creator { get; set; }
        public List<GroupPermission>? GroupPermissions { get; set; }
    }
    public class GroupPermission
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public bool CanModify { get; set; }
        public bool CanRead { get; set; }
    }
}
