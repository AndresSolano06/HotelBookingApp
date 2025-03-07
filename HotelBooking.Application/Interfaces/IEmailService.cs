using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendReservationEmail(string email, string fullName, string hotelName, DateTime CheckInDate, DateTime CheckOutDate, decimal totalPrice);
    }
}
