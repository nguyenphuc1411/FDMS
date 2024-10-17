namespace FDMS_API.Models.DTOs
{
    public class TypeDTO
    {
        public string TypeName { get; set; }
        public string Note { get; set; }
        public List<PermissionDTO>? Permissions { get; set; }
    }
}
