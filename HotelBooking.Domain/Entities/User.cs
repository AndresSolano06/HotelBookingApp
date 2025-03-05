using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.Entities
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the username for login.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hashed password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role of the user (e.g., admin, guest).
        /// </summary>
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = "guest";

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
    }
}
