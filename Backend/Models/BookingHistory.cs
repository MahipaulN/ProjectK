using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PROJECT_K.Models
{
    public class BookingHistory
    {
        [Key]
        public int Id {get; set;}
        // [ForeignKey("Users")]
        // public Guid Guid {get;set;}
        // [ForeignKey("Properties")]
        // public Guid PropertyId {get;set;}
        public Guid Users {get;set;}
        public Guid Properties {get;set;}
        public string? Facing {get; set;}
        public double TotalAmount {get; set;}
        public double PendingAmount {get; set;}
    }
}