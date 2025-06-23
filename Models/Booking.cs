using System;
using System.ComponentModel.DataAnnotations;

namespace EventVenueBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string EventId1 { get; set; }

        [Required]
        public string VenueId1 { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        // Navigation properties (no [ForeignKey] attributes needed)
        public Event Event { get; set; }
        public Venue Venue { get; set; }
    }
}