namespace FDMS_API.Data.Models
{
    public class DocumentPermission
    {
        // Foreign property
        public int DocumentID { get; set; }
        public int GroupID { get; set; }

        // Foreign Key Link
        public Document Document { get; set; }
        public GroupPermission GroupPermission { get; set; }
    }
}
