using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly HotelBookingDbContext _context;

        public ReservationService(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            if (room == null)
            {
                throw new ArgumentException("The specified RoomId does not exist.");
            }

            reservation.Room = room; // Asignar la habitación antes de guardar

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }


        public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
        {
            var existingReservation = await _context.Reservations.FindAsync(id);
            if (existingReservation == null) return null;

            existingReservation.CheckIn = reservation.CheckIn;
            existingReservation.CheckOut = reservation.CheckOut;
            existingReservation.TotalPrice = reservation.TotalPrice;

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
                                 .Include(r => r.Room)
                                 .ToListAsync();
        }
    }
}
