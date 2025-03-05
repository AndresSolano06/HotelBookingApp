using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Models
{
    /// <summary>
    /// Model for user registration request.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Username for the new user.
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Plain text password (will be hashed before storing).
        /// </summary>
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// User's email address.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Optional user role (defaults to "guest").
        /// </summary>
        public string Role { get; set; } = "guest";
    }
}
