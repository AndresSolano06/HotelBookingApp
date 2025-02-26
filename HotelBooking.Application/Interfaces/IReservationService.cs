using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(int roomId);
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<Reservation> UpdateReservationAsync(int id, Reservation reservation);
        Task<bool> CancelReservationAsync(int id);
    }
}
