using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Gets or sets the associated room entity.
        /// </summary>
        public Room Room { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of guests associated with the reservation.
        /// </summary>
        public List<Guest> Guests { get; set; } = new();

        /// <summary>
        /// Gets or sets the check-in date for the reservation.
        /// </summary>
        [Required]
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Gets or sets the check-out date for the reservation.
        /// </summary>
        [Required]
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Gets or sets the emergency contact name.
        /// </summary>
        [Required]
        public string EmergencyContactName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the emergency contact phone number.
        /// </summary>
        [Required]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total price of the reservation.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
