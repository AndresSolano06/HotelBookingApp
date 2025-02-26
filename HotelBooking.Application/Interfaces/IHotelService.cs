using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    /// <summary>
    /// Service interface for managing hotel operations.
    /// </summary>
    public interface IHotelService
    {
        /// <summary>
        /// Retrieves all hotels, optionally including inactive hotels.
        /// </summary>
        /// <param name="includeInactive">Set to true to include inactive hotels.</param>
        /// <returns>A list of hotels.</returns>
        Task<IEnumerable<Hotel>> GetAllHotelsAsync(bool includeInactive);

        /// <summary>
        /// Retrieves a hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <returns>The hotel object if found; otherwise, null.</returns>
        Task<Hotel> GetHotelByIdAsync(int id);

        /// <summary>
        /// Creates a new hotel record.
        /// </summary>
        /// <param name="hotel">The hotel object containing the details.</param>
        /// <returns>The newly created hotel.</returns>
        Task<Hotel> CreateHotelAsync(Hotel hotel);

        /// <summary>
        /// Updates an existing hotel.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <param name="hotel">The updated hotel details.</param>
        /// <returns>The updated hotel if successful; otherwise, null.</returns>
        Task<Hotel> UpdateHotelAsync(int id, Hotel hotel);

        /// <summary>
        /// Deletes a hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <returns>True if deleted successfully; otherwise, false.</returns>
        Task<bool> DeleteHotelAsync(int id);

        /// <summary>
        /// Updates an existing hotel without requiring an ID.
        /// </summary>
        /// <param name="hotel">The updated hotel details.</param>
        /// <returns>The updated hotel if successful.</returns>
        Task<Hotel> UpdateHotelAsync(Hotel hotel);

        /// <summary>
        /// Searches hotels based on city, check-in date, check-out date, and number of guests.
        /// </summary>
        /// <param name="city">City where the hotel is located (optional).</param>
        /// <param name="checkIn">Check-in date (optional).</param>
        /// <param name="checkOut">Check-out date (optional).</param>
        /// <param name="guests">Number of guests (optional).</param>
        /// <returns>A list of hotels that match the search criteria.</returns>
        Task<IEnumerable<Hotel>> SearchHotelsAsync(string? city, DateTime? checkIn, DateTime? checkOut, int? guests);


    }
}
