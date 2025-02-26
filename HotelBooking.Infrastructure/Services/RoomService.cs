using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly HotelBookingDbContext _context;

        public RoomService(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.Include(r => r.Hotel).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room> CreateRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room> UpdateRoomAsync(int id, Room room)
        {
            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom == null) return null;

            existingRoom.Type = room.Type;
            existingRoom.BasePrice = room.BasePrice;
            existingRoom.Taxes = room.Taxes;
            existingRoom.Location = room.Location;
            existingRoom.IsActive = room.IsActive;

            await _context.SaveChangesAsync();
            return existingRoom;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Room>> GetRoomsByHotelAsync(int hotelId)
        {
            return await _context.Rooms
                                 .Where(r => r.HotelId == hotelId)
                                 .ToListAsync();
        }
    }
}
