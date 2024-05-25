using System.ComponentModel.DataAnnotations;

namespace PROJECT_K.Models
{
    public class Properties
    {
        [Key]
        public Guid PropertyId {get; set;}
        public string? PropertyName {get; set;}
        public string? Address {get; set;}
        public string? City {get; set;}
        public string? Pincode {get; set;}
        public string? State {get; set;}
        public int TotalPlots {get; set;}
        public int WestFacingPlots {get; set;}
        public int EastFacingPlots {get; set;}
        public double EFPrice {get; set;}
        public double WFPrice {get; set;}
    }
}