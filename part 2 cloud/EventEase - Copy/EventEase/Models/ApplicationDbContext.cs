using Microsoft.EntityFrameworkCore;
using EventEase.Models; // Keep this line
using EventEase.Migrations;



namespace EventEase.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingViewModel> BookingViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BookingViewModel to map to the database view (if you created one)
            modelBuilder.Entity<BookingViewModel>().ToView("v_BookingDetails").HasNoKey();
        }
    }
}