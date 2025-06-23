using Microsoft.EntityFrameworkCore;
using EventVenueBookingSystem.Models;

namespace EventVenueBookingSystem.Data
{
    public class EventVenueBookingSystemDbContext : DbContext
    {
        public EventVenueBookingSystemDbContext(DbContextOptions<EventVenueBookingSystemDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany()
                .HasForeignKey(b => b.EventId1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany()
                .HasForeignKey(b => b.VenueId1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany()
                .HasForeignKey(e => e.EventTypeId);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.VenueId1, b.BookingDate, b.TimeSlot })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}