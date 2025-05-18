using System.ComponentModel.DataAnnotations;
using EventEase.Models;

namespace EventEase.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event Name is required.")]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event Date is required.")]
        public DateTime EventDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public int? VenueId { get; set; }

        public Venue? Venue { get; set; } // Nullable property

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}