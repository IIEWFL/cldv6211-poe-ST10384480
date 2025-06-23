using System.ComponentModel.DataAnnotations;

namespace EventVenueBookingSystem.Models
{
    public class EventType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";
    }
}