using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendReservationEmail(string email, string fullName, string hotelName, DateTime checkIn, DateTime checkOut);
    }
}
