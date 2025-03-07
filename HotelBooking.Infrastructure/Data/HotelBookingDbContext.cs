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
        /// Initializes a new instance of the <see cref="HotelBookingDbContext"/> class with specified options.
        /// </summary>
        /// <param name="options">The database context options, typically including the connection string and provider.</param>
        public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the collection of hotels in the system.
        /// </summary>
        public DbSet<Hotel> Hotels { get; set; }

        /// <summary>
        /// Gets or sets the collection of rooms in various hotels.
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the collection of reservations made by guests.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// Gets or sets the collection of guests who have booked reservations.
        /// </summary>
        public DbSet<Guest> Guests { get; set; }

        /// <summary>
        /// Gets or sets the collection of users, used for authentication and authorization.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Configures entity relationships, constraints, and database-specific settings.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entity relationships and constraints.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-Many: A hotel can have multiple rooms.
            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures rooms are deleted when a hotel is removed.

            // One-to-Many: A room belongs to a single hotel.
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents accidental deletion of related hotels.

            // One-to-Many: A room can have multiple reservations.
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Ensures reservations remain even if a room is removed.

            // One-to-Many: A reservation can have multiple guests.
            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Guests)
                .WithOne(g => g.Reservation)
                .HasForeignKey(g => g.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // Guests are deleted when a reservation is removed.

            // Configure decimal precision for price-related fields to avoid floating point issues.
            modelBuilder.Entity<Room>()
                .Property(r => r.BasePrice)
                .HasPrecision(18, 2); // Stores values with up to 18 digits, including 2 decimal places.

            modelBuilder.Entity<Room>()
                .Property(r => r.Taxes)
                .HasPrecision(18, 2); // Ensures proper storage of tax values to prevent rounding errors.

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(18, 2); // Guarantees accurate calculations for reservation totals.

            // Ensure that usernames in the Users table are unique to prevent duplicate accounts.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
