namespace FDMS_API.Data.Models
{
    public class Type_GroupPermission
    {

        public int Permission {  get; set; }
        // Foreign Property
        public int TypeID { get; set; }
        public int GroupID { get; set; }

        // Foreign Key Link
        public DocumentType DocumentType { get; set; }
        public GroupPermission GroupPermission { get; set; }
    }
}
