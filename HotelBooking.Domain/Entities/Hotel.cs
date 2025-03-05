using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.Entities
{
    /// <summary>
    /// Represents a hotel entity in the system.
    /// </summary>
    public class Hotel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the hotel.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the hotel.
        /// </summary>
        /// <remarks>This field is required.</remarks>
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the address of the hotel.
        /// </summary>
        /// <remarks>This field is required.</remarks>
        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city where the hotel is located.
        /// </summary>
        /// <remarks>This field is required.</remarks>
        [Required(ErrorMessage = "The City field is required.")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the hotel is active.
        /// </summary>
        /// <value>
        /// <c>true</c> if the hotel is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the list of rooms associated with the hotel.
        /// </summary>
        /// <value>A collection of <see cref="Room"/> instances.</value>
        public List<Room> Rooms { get; set; } = new();
    }
}
