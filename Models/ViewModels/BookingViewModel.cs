using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventVenueBookingSystem.Models.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string EventId1 { get; set; }

        [Required]
        public string VenueId1 { get; set; }

        public string EventName { get; set; }
        public string VenueName { get; set; }
        public string EventType { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        public IEnumerable<SelectListItem> Events { get; set; }
        public IEnumerable<SelectListItem> Venues { get; set; }
    }
}