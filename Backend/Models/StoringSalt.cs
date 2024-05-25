using System.ComponentModel.DataAnnotations;

namespace PROJECT_K.Models
{
    public class StoringSalt
    {
        [Key]
        public Guid Guid {get;set;}
        public byte[] Salt {get;set;}
    }
}