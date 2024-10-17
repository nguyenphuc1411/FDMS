namespace FDMS_API.Models.DTOs
{
    public class GroupDTO
    {
        public string GroupName { get; set; }
        public string Note {  get; set; }
        public List<int>? UserIDs { get; set; }
    }
}
