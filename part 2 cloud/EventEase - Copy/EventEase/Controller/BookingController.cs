using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings.Include(b => b.Event).Include(b => b.Venue).ToListAsync();
            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.Include(b => b.Event).Include(b => b.Venue).FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Booking/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Events = await _context.Events.ToListAsync();
            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            // Check for double booking
            if (await IsVenueBooked(booking.VenueId, booking.EventId, booking.BookingDate, 0)) // 0 for new booking
            {
                ModelState.AddModelError(string.Empty, "This venue is already booked for another event on this date.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Events = await _context.Events.ToListAsync();
            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewBag.Events = await _context.Events.ToListAsync();
            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            // Check for double booking, excluding the current booking being edited
            if (await IsVenueBooked(booking.VenueId, booking.EventId, booking.BookingDate, id))
            {
                ModelState.AddModelError(string.Empty, "This venue is already booked for another event on this date.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Events = await _context.Events.ToListAsync();
            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.Include(b => b.Event).Include(b => b.Venue).FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IsVenueBooked(int venueId, int eventId, DateTime bookingDate, int currentBookingId)
        {
            return await _context.Bookings
                .Where(b => b.VenueId == venueId &&
                            b.BookingDate.Date == bookingDate.Date &&
                            b.EventId != eventId && // Ensure it's a different event
                            b.BookingId != currentBookingId) // Exclude the current booking if editing
                .AnyAsync();
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
        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}