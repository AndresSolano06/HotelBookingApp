using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Services
{
    public class HotelService : IHotelService
    {
        private readonly HotelBookingDbContext _context;

        public HotelService(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels.Include(h => h.Rooms).ToListAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Hotel> CreateHotelAsync(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

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

        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return false;

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
