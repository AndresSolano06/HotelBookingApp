using HotelBooking.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for hotel-related operations.
    /// </summary>
    public interface IHotelService
    {
        /// <summary>
        /// Retrieves a list of all hotels.
        /// </summary>
        /// <param name="includeInactive">Indicates whether to include inactive hotels.</param>
        /// <returns>A collection of hotels.</returns>
        Task<IEnumerable<Hotel>> GetAllHotelsAsync(bool includeInactive);

        /// <summary>
        /// Retrieves a hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <returns>The hotel entity if found; otherwise, null.</returns>
        Task<Hotel> GetHotelByIdAsync(int id);

        /// <summary>
        /// Creates a new hotel entry.
        /// </summary>
        /// <param name="hotel">The hotel entity to be created.</param>
        /// <returns>The created hotel entity.</returns>
        Task<Hotel> CreateHotelAsync(Hotel hotel);

        /// <summary>
        /// Updates an existing hotel.
        /// </summary>
        /// <param name="id">The ID of the hotel to be updated.</param>
        /// <param name="hotel">The updated hotel data.</param>
        /// <returns>The updated hotel entity.</returns>
        Task<Hotel> UpdateHotelAsync(int id, Hotel hotel);

        /// <summary>
        /// Deletes a hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel to be deleted.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteHotelAsync(int id);
    }
}
