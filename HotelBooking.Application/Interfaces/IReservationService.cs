using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for reservation-related operations.
    /// </summary>
    public interface IReservationService
    {
        /// <summary>
        /// Retrieves all reservations associated with a specific room.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room.</param>
        /// <returns>A collection of reservations for the specified room.</returns>
        Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(int roomId);

        /// <summary>
        /// Retrieves all reservations in the system.
        /// </summary>
        /// <returns>A collection of all reservations.</returns>
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();

        /// <summary>
        /// Retrieves a reservation by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation.</param>
        /// <returns>The reservation entity if found; otherwise, null.</returns>
        Task<Reservation> GetReservationByIdAsync(int id);

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="reservation">The reservation entity to be created.</param>
        /// <returns>The created reservation entity.</returns>
        Task<Reservation> CreateReservationAsync(Reservation reservation);

        /// <summary>
        /// Updates an existing reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to be updated.</param>
        /// <param name="reservation">The updated reservation data.</param>
        /// <returns>The updated reservation entity.</returns>
        Task<Reservation> UpdateReservationAsync(int id, Reservation reservation);

        /// <summary>
        /// Checks if there is a conflicting reservation for the same room within the given check-in and check-out dates.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation to check (used for updates).</param>
        /// <param name="roomId">The ID of the room.</param>
        /// <param name="CheckInDate">The check-in date.</param>
        /// <param name="CheckOutDate">The check-out date.</param>
        /// <returns>True if a conflict exists; otherwise, false.</returns>
        Task<bool> ExistsConflictReservationAsync(int reservationId, int roomId, DateTime CheckInDate, DateTime CheckOutDate); 

        /// <summary>
        /// Cancels a reservation by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation to be canceled.</param>
        /// <returns>True if the cancellation was successful; otherwise, false.</returns>
        Task<bool> CancelReservationAsync(int id);

        /// <summary>
        /// Checks if a room is already booked for a specific date range.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room.</param>
        /// <param name="CheckInDate">The check-in date.</param>
        /// <param name="CheckOutDate">The check-out date.</param>
        /// <returns>True if the room is booked; otherwise, false.</returns>
        Task<bool> IsRoomBookedAsync(int roomId, DateTime CheckInDate, DateTime CheckOutDate);
    }
}
