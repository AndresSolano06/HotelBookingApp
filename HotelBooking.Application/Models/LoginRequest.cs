using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Models
{
    /// <summary>
    /// Represents the request model for user authentication.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The username of the user.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The password of the user.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
