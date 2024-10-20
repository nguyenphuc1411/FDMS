namespace FDMS_API.Models.DTOs.Type
{
    public class GetTypes
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Creator { get; set; }
        public int TotalGroups {  get; set; }
    }
}
