using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.Entities
{
    /// <summary>
    /// Represents a guest in a reservation.
    /// </summary>
    public class Guest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the guest.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the guest.
        /// </summary>
        [Required(ErrorMessage = "Guest full name is required.")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the contact email of the guest.
        /// </summary>
        [Required(ErrorMessage = "Guest email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth of the guest.
        /// </summary>
        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the gender of the guest.
        /// </summary>
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of identification document.
        /// </summary>
        [Required(ErrorMessage = "Document type is required.")]
        public string DocumentType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the document number of the guest.
        /// </summary>
        [Required(ErrorMessage = "Document number is required.")]
        public string DocumentNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number of the guest.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the reservation ID associated with the guest.
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// Navigation property for the associated reservation.
        /// </summary>
        [JsonIgnore]
        public Reservation? Reservation { get; set; }
    }
}
