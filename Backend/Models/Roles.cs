namespace PROJECT_K.Models
{
    public class Roles
    {
        public int Id {get; set;}
        public string RoleName {get; set;}
        public ICollection<Users> Users{get;set;}
    }
}