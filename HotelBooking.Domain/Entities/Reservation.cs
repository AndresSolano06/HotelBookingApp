using HotelBooking.Domain.Entities;
using System;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.Entities
{
    /// <summary>
    /// Represents a reservation for a hotel room.
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// Gets or sets the unique identifier of the reservation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the room being reserved.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the guest making the reservation.
        /// </summary>
        public string GuestFullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the guest making the reservation.
        /// </summary>
        public string GuestEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the check-in date of the reservation.
        /// </summary>
        public DateTime CheckIn { get; set; }

        /// <summary>
        /// Gets or sets the check-out date of the reservation.
        /// </summary>
        public DateTime CheckOut { get; set; }

        /// <summary>
        /// Gets or sets the total price of the reservation.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the room associated with the reservation.
        /// </summary>
        /// <remarks>
        /// This property is ignored in JSON serialization to avoid circular references.
        /// </remarks>
        [JsonIgnore]
        public Room? Room { get; set; } = null!;
    }
}
