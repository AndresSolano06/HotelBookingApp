using HotelBooking.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using HotelBooking.Infrastructure.Settings;

namespace HotelBooking.Infrastructure.Services
{
    /// <summary>
    /// Service for sending email notifications.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="emailSettings">The email settings configuration.</param>
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        /// <summary>
        /// Sends a reservation confirmation email.
        /// </summary>
        /// <param name="toEmail">Recipient's email address.</param>
        /// <param name="guestName">Guest's full name.</param>
        /// <param name="hotelName">Hotel's name.</param>
        /// <param name="CheckInDate">Check-in date.</param>
        /// <param name="CheckOutDate">Check-out date.</param>
        public async Task SendReservationEmail(string toEmail, string guestName, string hotelName, DateTime CheckInDate, DateTime CheckOutDate, decimal totalPrice)
        {
            try
            {
                using (var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                    smtp.EnableSsl = _emailSettings.EnableSSL;

                    var message = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                        Subject = "Reservation Confirmation",
                        Body = $"Hello {guestName},\n\nYour reservation at {hotelName} is confirmed.\nCheck-in: {CheckInDate}\nCheck-out: {CheckOutDate}\nTotal Price: {totalPrice}\n\nThank you for choosing us!",
                        IsBodyHtml = false
                    };

                    message.To.Add(toEmail);

                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
