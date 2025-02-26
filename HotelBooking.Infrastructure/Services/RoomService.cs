using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Services
{
    /// <summary>
    /// Provides services for managing rooms in the system.
    /// </summary>
    public class RoomService : IRoomService
    {
        private readonly HotelBookingDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomService"/> class.
        /// </summary>
        /// <param name="context">Database context for hotel booking.</param>
        public RoomService(HotelBookingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all rooms, including their associated hotels.
        /// </summary>
        /// <returns>A list of all rooms.</returns>
        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.Include(r => r.Hotel).ToListAsync();
        }

        /// <summary>
        /// Retrieves a room by its ID, including the associated hotel.
        /// </summary>
        /// <param name="id">The room ID.</param>
        /// <returns>The room if found; otherwise, null.</returns>
        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Creates a new room.
        /// </summary>
        /// <param name="room">The room details.</param>
        /// <returns>The newly created room.</returns>
        public async Task<Room> CreateRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        /// <summary>
        /// Updates an existing room.
        /// </summary>
        /// <param name="id">The ID of the room to update.</param>
        /// <param name="room">The updated room details.</param>
        /// <returns>The updated room if successful; otherwise, null.</returns>
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

        /// <summary>
        /// Deletes a room by removing it from the database.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all active rooms for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>A list of active rooms for the specified hotel.</returns>
        public async Task<IEnumerable<Room>> GetRoomsByHotelAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.IsActive)
                .ToListAsync();
        }

        /// <summary>
        /// Updates the active status of a room.
        /// </summary>
        /// <param name="room">The room to update.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateRoomAsync(Room room)
        {
            var existingRoom = await _context.Rooms.FindAsync(room.Id);
            if (existingRoom == null)
            {
                return false;
            }

            existingRoom.IsActive = room.IsActive;

            _context.Rooms.Update(existingRoom);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
