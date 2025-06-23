using System.ComponentModel.DataAnnotations;

namespace EventVenueBookingSystem.Models
{
    public class Venue
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Generate unique ID

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Location { get; set; } = "";

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}