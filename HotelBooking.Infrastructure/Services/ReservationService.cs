using HotelBooking.Application.Interfaces;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ReservationService(HotelBookingDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Guests)
                .ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Guests)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == reservation.RoomId);

            if (room == null)
            {
                throw new ArgumentException("The specified RoomId does not exist.");
            }

            if (!room.IsActive)
            {
                throw new ArgumentException("The selected room is not available for booking.");
            }

            if (reservation.CheckInDate < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Check-in date cannot be in the past.");
            }

            if (reservation.CheckOutDate <= reservation.CheckInDate)
            {
                throw new ArgumentException("Check-out date must be after the check-in date.");
            }

            if (room.Capacity < reservation.Guests.Count)
            {
                throw new ArgumentException("The selected room does not have enough capacity.");
            }

            if (await IsRoomBookedAsync(reservation.RoomId, reservation.CheckInDate, reservation.CheckOutDate))
            {
                throw new ArgumentException("This room is already booked for the selected dates.");
            }

            foreach (var guest in reservation.Guests)
            {
                if (guest.DateOfBirth > DateTime.UtcNow)
                {
                    throw new ArgumentException($"Guest {guest.FirstName} {guest.LastName} has an invalid Date of Birth. It cannot be in the future.");
                }
            }

            int days = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            reservation.TotalPrice = (room.BasePrice + room.Taxes) * days;

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            if (reservation.Guests.Any())
            {
                var primaryGuest = reservation.Guests.First();
                await _emailService.SendReservationEmail(
                    primaryGuest.Email,
                    $"{primaryGuest.FirstName}  {primaryGuest.LastName}",
                    room.Hotel.Name,
                    reservation.CheckInDate,
                    reservation.CheckOutDate,
                    reservation.TotalPrice
                );
            }

            return reservation;
        }

        public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
        {
            var existingReservation = await _context.Reservations
                .Include(r => r.Guests)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingReservation == null) return null;

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == reservation.RoomId);
            if (room == null)
            {
                throw new ArgumentException("The specified RoomId does not exist.");
            }

            if (!room.IsActive)
            {
                throw new ArgumentException("The selected room is not available for booking.");
            }

            if (await ExistsConflictReservationAsync(id, reservation.RoomId, reservation.CheckInDate, reservation.CheckOutDate))
            {
                throw new ArgumentException("This room is already booked for the selected dates.");
            }

            foreach (var guest in reservation.Guests)
            {
                if (guest.DateOfBirth > DateTime.UtcNow)
                {
                    throw new ArgumentException($"Guest {guest.FirstName} {guest.LastName} has an invalid Date of Birth. It cannot be in the future.");
                }
            }

            int days = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            existingReservation.TotalPrice = (room.BasePrice + room.Taxes) * days;

            existingReservation.CheckInDate = reservation.CheckInDate;
            existingReservation.CheckOutDate = reservation.CheckOutDate;
            existingReservation.EmergencyContactName = reservation.EmergencyContactName;
            existingReservation.EmergencyContactPhone = reservation.EmergencyContactPhone;

            _context.Guests.RemoveRange(existingReservation.Guests);
            existingReservation.Guests = reservation.Guests;

            await _context.SaveChangesAsync();
            return existingReservation;
        }


        public async Task<bool> CancelReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(int roomId)
        {
            return await _context.Reservations
                .Where(r => r.RoomId == roomId)
                .Include(r => r.Guests)
                .ToListAsync();
        }

        public async Task<bool> IsRoomBookedAsync(int roomId, DateTime CheckInDate, DateTime CheckOutDate)
        {
            return await _context.Reservations
                .AnyAsync(r => r.RoomId == roomId &&
                              ((CheckInDate >= r.CheckInDate && CheckInDate < r.CheckOutDate) ||
                               (CheckOutDate > r.CheckInDate && CheckOutDate <= r.CheckOutDate) ||
                               (CheckInDate <= r.CheckInDate && CheckOutDate >= r.CheckOutDate)));
        }

        public async Task<bool> ExistsConflictReservationAsync(int reservationId, int roomId, DateTime CheckInDate, DateTime CheckOutDate)
        {
            return await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.Id != reservationId &&
                (
                    (CheckInDate >= r.CheckInDate && CheckInDate < r.CheckOutDate) ||
                    (CheckOutDate > r.CheckInDate && CheckOutDate <= r.CheckOutDate) ||
                    (CheckInDate <= r.CheckInDate && CheckOutDate >= r.CheckOutDate)
                )
            );
        }
    }
}
