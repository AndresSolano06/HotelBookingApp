using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.Entities
{
    /// <summary>
    /// Represents a reservation made by a guest for a specific room.
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the room associated with this reservation.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the check-in date for the reservation.
        /// </summary>
        [Required(ErrorMessage = "Check-in date is required.")]
        public DateTime CheckIn { get; set; }

        /// <summary>
        /// Gets or sets the check-out date for the reservation.
        /// </summary>
        [Required(ErrorMessage = "Check-out date is required.")]
        public DateTime CheckOut { get; set; }

        /// <summary>
        /// Gets or sets the total price of the reservation.
        /// </summary>
        [Required(ErrorMessage = "Total price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the list of guests associated with the reservation.
        /// </summary>
        public List<Guest> Guests { get; set; } = new();

        /// <summary>
        /// Navigation property for the associated room.
        /// </summary>
        [JsonIgnore]
        public Room? Room { get; set; }
    }
}
