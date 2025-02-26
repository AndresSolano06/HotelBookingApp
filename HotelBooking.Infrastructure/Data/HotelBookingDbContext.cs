using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data
{
    public class HotelBookingDbContext : DbContext
    {
        public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId);


            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomId);

            // Corrección de precisión para valores `decimal`
            modelBuilder.Entity<Room>()
                .Property(r => r.BasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Room>()
                .Property(r => r.Taxes)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(18, 2);
        }
    }
}
