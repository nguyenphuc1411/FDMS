
namespace FDMS_API.Data.Models
{
    public class GroupUser
    {    
        // Foreign Property
        public int UserID {  get; set; }
        public int GroupID {  get; set; }

        // Foreign Key Link
        public User User { get; set; }
        public Group Group { get; set; }
    }
}
