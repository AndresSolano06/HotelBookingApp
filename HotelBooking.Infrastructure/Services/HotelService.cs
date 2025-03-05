using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Services
{
    /// <summary>
    /// Provides services for managing hotels in the system.
    /// </summary>
    public class HotelService : IHotelService
    {
        private readonly HotelBookingDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelService"/> class.
        /// </summary>
        /// <param name="context">Database context for hotel booking.</param>
        public HotelService(HotelBookingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all hotels, with an option to include inactive ones.
        /// </summary>
        /// <param name="includeInactive">If true, includes inactive hotels.</param>
        /// <returns>A list of hotels.</returns>
        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync(bool includeInactive)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)  // Incluir las habitaciones
                .Where(h => includeInactive || h.IsActive)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves a hotel by its ID, including its rooms.
        /// </summary>
        /// <param name="id">The hotel ID.</param>
        /// <returns>The hotel if found; otherwise, null.</returns>
        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        /// <summary>
        /// Creates a new hotel and saves it to the database.
        /// </summary>
        /// <param name="hotel">The hotel entity to be created.</param>
        /// <returns>The newly created hotel.</returns>
        public async Task<Hotel> CreateHotelAsync(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        /// <summary>
        /// Updates an existing hotel with new information.
        /// </summary>
        /// <param name="id">The ID of the hotel to be updated.</param>
        /// <param name="hotel">The updated hotel entity.</param>
        /// <returns>The updated hotel if found; otherwise, null.</returns>
        public async Task<Hotel> UpdateHotelAsync(int id, Hotel hotel)
        {
            var existingHotel = await _context.Hotels.FindAsync(id);
            if (existingHotel == null) return null;

            existingHotel.Name = hotel.Name;
            existingHotel.Address = hotel.Address;
            existingHotel.City = hotel.City;
            existingHotel.IsActive = hotel.IsActive;

            await _context.SaveChangesAsync();
            return existingHotel;
        }

        /// <summary>
        /// Deletes a hotel by its ID.
        /// </summary>
        /// <param name="id">The ID of the hotel to be deleted.</param>
        /// <returns>True if deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return false;

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates a hotel entity in the database.
        /// </summary>
        /// <param name="hotel">The hotel entity to update.</param>
        /// <returns>The updated hotel.</returns>
        public async Task<Hotel> UpdateHotelAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }
        /// <summary>
        /// Searches for available hotels in a specific city with open rooms for the given dates and number of guests.
        /// At least one filter must be provided.
        /// </summary>
        /// <param name="city">Optional. City to search hotels in.</param>
        /// <param name="CheckInDate">Optional. Check-in date.</param>
        /// <param name="CheckOutDate">Optional. Check-out date.</param>
        /// <param name="guests">Optional. Number of guests.</param>
        /// <returns>List of available hotels.</returns>
        public async Task<IEnumerable<Hotel>> SearchHotelsAsync(string? city, DateTime? CheckInDate, DateTime? CheckOutDate, int? guests)
        {
            var query = _context.Hotels
                .Include(h => h.Rooms.Where(r => r.IsActive))
                .Where(h => h.IsActive)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(h => h.City.ToLower() == city.ToLower());
            }

            if (guests > 0)
            {
                query = query.Where(h => h.Rooms.Any(r => r.IsActive && r.Capacity >= guests));
            }

            if (CheckInDate < DateTime.UtcNow.Date || CheckOutDate < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Search dates cannot be in the past.");
            }

            if (CheckInDate != null || CheckOutDate != null)
            {
                query = query.Where(h => h.Rooms.Any(r =>
                    !_context.Reservations.Any(res =>
                        res.RoomId == r.Id &&
                        (
                            (CheckInDate != null && CheckInDate >= res.CheckInDate && CheckInDate < res.CheckOutDate) ||
                            (CheckOutDate != null && CheckOutDate > res.CheckInDate && CheckOutDate <= res.CheckOutDate) ||
                            (CheckInDate != null && CheckOutDate != null && CheckInDate <= res.CheckInDate && CheckOutDate >= res.CheckOutDate)
                        )
                    )
                ));
            }

            if (guests != null && guests > 0)
            {
                query = query.Where(h => h.Rooms.Any(r => r.IsActive));
            }

            return await query.ToListAsync();
        }

    }
}
