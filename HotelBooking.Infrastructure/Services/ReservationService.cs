using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Services
{
    /// <summary>
    /// Provides services for managing reservations in the system.
    /// </summary>
    public class ReservationService : IReservationService
    {
        private readonly HotelBookingDbContext _context;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationService"/> class.
        /// </summary>
        /// <param name="context">Database context for hotel booking.</param>
        public ReservationService(HotelBookingDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// Retrieves all reservations, including guests.
        /// </summary>
        /// <returns>A list of all reservations.</returns>
        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Guests)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a reservation by its ID, including guests.
        /// </summary>
        /// <param name="id">The reservation ID.</param>
        /// <returns>The reservation if found; otherwise, null.</returns>
        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Guests)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Creates a new reservation, ensuring room capacity is not exceeded.
        /// </summary>
        /// <param name="reservation">The reservation details.</param>
        /// <returns>The newly created reservation.</returns>
        /// <exception cref="ArgumentException">Thrown if the RoomId does not exist or if room capacity is exceeded.</exception>
        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            if (room == null)
            {
                throw new ArgumentException("The specified RoomId does not exist.");
            }

            if (room.Capacity < reservation.Guests.Count)
            {
                throw new ArgumentException("The selected room does not have enough capacity.");
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            if (reservation.Guests.Any())
            {
                var primaryGuest = reservation.Guests.First();
                await _emailService.SendReservationEmail(
                    primaryGuest.Email,
                    primaryGuest.FullName,
                    room.Hotel.Name,
                    reservation.CheckIn,
                    reservation.CheckOut
                );
            }
            return reservation;
        }

        /// <summary>
        /// Updates an existing reservation, including guests and emergency contact details.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="reservation">The updated reservation details.</param>
        /// <returns>The updated reservation if successful; otherwise, null.</returns>
        public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
        {
            var existingReservation = await _context.Reservations
                .Include(r => r.Guests)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingReservation == null) return null;

            existingReservation.CheckIn = reservation.CheckIn;
            existingReservation.CheckOut = reservation.CheckOut;
            existingReservation.TotalPrice = reservation.TotalPrice;
            existingReservation.EmergencyContactName = reservation.EmergencyContactName;
            existingReservation.EmergencyContactPhone = reservation.EmergencyContactPhone;

            // Update guests
            _context.Guests.RemoveRange(existingReservation.Guests);
            existingReservation.Guests = reservation.Guests;

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
                .Include(r => r.Guests)
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
                    (checkOut > r.CheckIn && checkOut <= r.CheckOut) ||
                    (checkIn <= r.CheckIn && checkOut >= r.CheckOut)
                )
            );
        }
    }
}
