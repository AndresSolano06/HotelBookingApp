using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Services
{
    /// <summary>
    /// Provides services for managing reservations in the system.
    /// </summary>
    public class ReservationService : IReservationService
    {
        private readonly HotelBookingDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationService"/> class.
        /// </summary>
        /// <param name="context">Database context for hotel booking.</param>
        public ReservationService(HotelBookingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all reservations, including their associated rooms.
        /// </summary>
        /// <returns>A list of all reservations.</returns>
        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a reservation by its ID, including its associated room.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>The reservation if found; otherwise, null.</returns>
        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="reservation">The reservation details.</param>
        /// <returns>The newly created reservation.</returns>
        /// <exception cref="ArgumentException">Thrown if the RoomId does not exist.</exception>
        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            if (room == null)
            {
                throw new ArgumentException("The specified RoomId does not exist.");
            }

            reservation.Room = room;

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        /// <summary>
        /// Updates an existing reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="reservation">The updated reservation details.</param>
        /// <returns>The updated reservation if successful; otherwise, null.</returns>
        public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
        {
            var existingReservation = await _context.Reservations.FindAsync(id);
            if (existingReservation == null) return null;

            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            if (room == null) return null;

            bool conflict = await _context.Reservations.AnyAsync(r =>
                r.RoomId == reservation.RoomId &&
                r.Id != id &&
                (
                    (reservation.CheckIn >= r.CheckIn && reservation.CheckIn < r.CheckOut) ||
                    (reservation.CheckOut > r.CheckIn && reservation.CheckOut <= r.CheckOut)
                )
            );

            if (conflict) return null;

            existingReservation.RoomId = reservation.RoomId;
            existingReservation.GuestFullName = reservation.GuestFullName;
            existingReservation.GuestEmail = reservation.GuestEmail;
            existingReservation.CheckIn = reservation.CheckIn;
            existingReservation.CheckOut = reservation.CheckOut;
            existingReservation.TotalPrice = reservation.TotalPrice;

            await _context.SaveChangesAsync();
            return existingReservation;
        }

        /// <summary>
        /// Cancels a reservation by removing it from the database.
        /// </summary>
        /// <param name="id">The ID of the reservation to cancel.</param>
        /// <returns>True if the cancellation was successful; otherwise, false.</returns>
        public async Task<bool> CancelReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves reservations for a specific room.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <returns>A list of reservations for the specified room.</returns>
        public async Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(int roomId)
        {
            return await _context.Reservations
                                 .Where(r => r.RoomId == roomId)
                                 .Include(r => r.Room)
                                 .ToListAsync();
        }

        /// <summary>
        /// Checks if a room is already booked for the specified date range.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <param name="checkIn">Check-in date.</param>
        /// <param name="checkOut">Check-out date.</param>
        /// <returns>True if the room is booked; otherwise, false.</returns>
        public async Task<bool> IsRoomBookedAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            return await _context.Reservations
                .AnyAsync(r => r.RoomId == roomId &&
                              ((checkIn >= r.CheckIn && checkIn < r.CheckOut) ||
                               (checkOut > r.CheckIn && checkOut <= r.CheckOut) ||
                               (checkIn <= r.CheckIn && checkOut >= r.CheckOut)));
        }

        /// <summary>
        /// Checks if a reservation conflicts with an existing booking.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation to check.</param>
        /// <param name="roomId">The ID of the room.</param>
        /// <param name="checkIn">Check-in date.</param>
        /// <param name="checkOut">Check-out date.</param>
        /// <returns>True if a conflict exists; otherwise, false.</returns>
        public async Task<bool> ExistsConflictReservationAsync(int reservationId, int roomId, DateTime checkIn, DateTime checkOut)
        {
            return await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.Id != reservationId &&
                (
                    (checkIn >= r.CheckIn && checkIn < r.CheckOut) ||
                    (checkOut > r.CheckIn && checkOut <= r.CheckOut)
                )
            );
        }
    }
}
