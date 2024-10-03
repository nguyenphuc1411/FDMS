
namespace FDMS_API.Data.Models
{
    public class User_Group
    {    
        public int UserID {  get; set; }
        public int GroupID {  get; set; }

        // Foreign Key Link
        public User User { get; set; }
        public GroupPermission GroupPermission { get; set; }
    }
}
