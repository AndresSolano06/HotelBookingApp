using System.Text.Json.Serialization;

namespace HotelBooking.Domain.Entities
{
    /// <summary>
    /// Represents a room within a hotel.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Gets or sets the unique identifier of the room.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the hotel the room belongs to.
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// Gets or sets the type of room (e.g., Single, Double, Suite).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the base price of the room per night.
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// Gets or sets the amount of taxes applied to the room price.
        /// </summary>
        public decimal Taxes { get; set; }

        /// <summary>
        /// Gets or sets the location of the room within the hotel (e.g., Floor 3, Room 302).
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the room is active and available for reservations.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of guests that the room can accommodate.
        /// </summary>
        /// <remarks>
        /// This property defines the room's capacity in terms of guests. It is used to filter available rooms 
        /// when searching for hotels based on the number of guests.
        /// </remarks>
        public int Capacity { get; set; }


        /// <summary>
        /// Gets or sets the hotel associated with this room.
        /// </summary>
        /// <remarks>
        /// This property is ignored in JSON serialization to avoid circular references.
        /// </remarks>
        [JsonIgnore]
        public Hotel? Hotel { get; set; }
    }
}
