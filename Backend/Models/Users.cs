using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PROJECT_K.Models
{
    public class Users
    {
        [Key]
        public Guid Guid{get; set;}
        public string? Name{get; set;}
        public string? Email{get; set;}
        public string? Password{get; set;}
        public string? Phone{get; set;}
        [ForeignKeyAttribute("UserStatus")]
        public int UserStatusId {get; set;} = 0;
        [ForeignKey("Roles")]
        public int RolesId{get;set;}
        // public ICollection<BookingHistory> BookingHistory{get;set;}
    }
}