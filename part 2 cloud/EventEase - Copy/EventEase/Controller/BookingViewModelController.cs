using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class BookingViewModelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingViewModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> EnhancedIndex(string SearchTerm)
        {
            IQueryable<BookingViewModel> bookingDetails = _context.BookingViewModels;

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                bookingDetails = bookingDetails.Where(b => b.BookingId.ToString().Contains(SearchTerm) ||
                                                       b.EventName.Contains(SearchTerm));
                ViewBag.SearchTerm = SearchTerm;
            }

            return View(await bookingDetails.ToListAsync());
        }

        // You might need Create, Edit, Delete actions here as well,
        // depending on how you want to manage BookingViewModels.
        // If you intend to modify the underlying Bookings, Events, or Venues,
        // you would typically use the standard BookingController, EventController,
        // and VenueController for those operations.
    }
}