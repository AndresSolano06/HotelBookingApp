using HotelBooking.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for room-related operations.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Retrieves all rooms associated with a specific hotel.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <returns>A collection of rooms belonging to the specified hotel.</returns>
        Task<IEnumerable<Room>> GetRoomsByHotelAsync(int hotelId);

        /// <summary>
        /// Retrieves all rooms in the system.
        /// </summary>
        /// <returns>A collection of all rooms.</returns>
        Task<IEnumerable<Room>> GetAllRoomsAsync();

        /// <summary>
        /// Retrieves a room by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the room.</param>
        /// <returns>The room entity if found; otherwise, null.</returns>
        Task<Room> GetRoomByIdAsync(int id);

        /// <summary>
        /// Creates a new room in the system.
        /// </summary>
        /// <param name="room">The room entity to be created.</param>
        /// <returns>The created room entity.</returns>
        Task<Room> CreateRoomAsync(Room room);

        /// <summary>
        /// Updates an existing room in the system.
        /// </summary>
        /// <param name="id">The ID of the room to be updated.</param>
        /// <param name="room">The updated room data.</param>
        /// <returns>The updated room entity.</returns>
        Task<Room> UpdateRoomAsync(int id, Room room);

        /// <summary>
        /// Deletes a room by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the room to be deleted.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteRoomAsync(int id);

        /// <summary>
        /// Updates an existing room's details.
        /// </summary>
        /// <param name="room">The updated room entity.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateRoomAsync(Room room);
    }
}
