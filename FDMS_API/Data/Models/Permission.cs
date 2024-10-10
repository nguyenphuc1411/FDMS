namespace FDMS_API.Data.Models
{
    public class Permission
    {

        public bool CanRead {  get; set; } = false;
        public bool CanModify { get; set; } = false;

        // Foreign Property
        public int TypeID { get; set; }
        public int GroupID { get; set; }

        // Foreign Key Link
        public Type Type { get; set; }
        public Group Group { get; set; }
    }
}
