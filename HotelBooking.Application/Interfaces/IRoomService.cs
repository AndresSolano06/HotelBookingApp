using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetRoomsByHotelAsync(int hotelId);
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task<Room> CreateRoomAsync(Room room);
        Task<Room> UpdateRoomAsync(int id, Room room);
        Task<bool> DeleteRoomAsync(int id);
    }
}
