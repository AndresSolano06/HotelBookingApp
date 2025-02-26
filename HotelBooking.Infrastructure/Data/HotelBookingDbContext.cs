using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context for the Hotel Booking system.
    /// </summary>
    public class HotelBookingDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HotelBookingDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the DbSet for hotels.
        /// </summary>
        public DbSet<Hotel> Hotels { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for rooms.
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for reservations.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for guests.
        /// </summary>
        public DbSet<Guest> Guests { get; set; }

        /// <summary>
        /// Configures entity relationships and constraints.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entity relationships.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-Many: A hotel can have multiple rooms
            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId);

            // One-to-Many: A room belongs to a single hotel
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId);

            // One-to-Many: A room can have multiple reservations
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomId);

            // One-to-Many: A reservation can have multiple guests
            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Guests)
                .WithOne(g => g.Reservation)
                .HasForeignKey(g => g.ReservationId);
        }
    }
}
