using System.ComponentModel.DataAnnotations;
using EventEase.Models;
using EventEase.Migrations;

namespace EventEase.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Event is required.")]
        public int EventId { get; set; }
        public Event? Event { get; set; }

        [Required(ErrorMessage = "Venue is required.")]
        public int VenueId { get; set; }
        public Venue? Venue { get; set; }

        [Required(ErrorMessage = "Booking Date is required.")]
        public DateTime BookingDate { get; set; }
    }
}