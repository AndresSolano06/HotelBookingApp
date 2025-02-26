using HotelBooking.Domain.Entities; // Asegúrate de que esta línea existe

namespace HotelBooking.Application.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel> GetHotelByIdAsync(int id);
        Task<Hotel> CreateHotelAsync(Hotel hotel);
        Task<Hotel> UpdateHotelAsync(int id, Hotel hotel);
        Task<bool> DeleteHotelAsync(int id);
    }
}
