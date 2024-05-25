using System.Collections.ObjectModel;

namespace PROJECT_K.Models
{
    public class UserStatus
    {
        public int Id {get; set;}
        public string? Status {get; set;}
        public ICollection<Users> Users{get;set;}
    }

}