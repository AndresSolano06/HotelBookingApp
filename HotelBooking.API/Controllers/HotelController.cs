using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.API.Controllers
{
    [Route("api/hotel")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Retrieves all hotels.
        /// </summary>
        /// <param name="includeInactive">Indicates whether to include inactive hotels.</param>
        /// <returns>List of hotels.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Hotel>), 200)]
        [ProducesResponseType(500)]
        [SwaggerResponse(200, "List of hotels retrieved successfully.", typeof(IEnumerable<Hotel>))]
        [SwaggerResponse(500, "Internal server error occurred.")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels([FromQuery] bool includeInactive = false)
        {
            var hotels = await _hotelService.GetAllHotelsAsync(includeInactive);
            return Ok(hotels);
        }

        /// <summary>
        /// Retrieves a hotel by its ID.
        /// </summary>
        /// <param name="id">Hotel ID.</param>
        /// <returns>Hotel details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Hotel found successfully.", typeof(Hotel))]
        [SwaggerResponse(404, "Hotel not found.")]
        public async Task<ActionResult<Hotel>> GetHotelById(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = $"Hotel with ID {id} was not found." });
            }
            return Ok(hotel);
        }

        /// <summary>
        /// Creates a new hotel.
        /// </summary>
        /// <param name="hotel">Hotel details.</param>
        /// <returns>Created hotel.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Hotel), 201)]
        [ProducesResponseType(400)]
        [SwaggerResponse(201, "Hotel created successfully.", typeof(Hotel))]
        [SwaggerResponse(400, "Bad request. Missing required fields.")]
        public async Task<ActionResult<Hotel>> CreateHotel([FromBody] Hotel hotel)
        {
            if (string.IsNullOrEmpty(hotel.Name) || string.IsNullOrEmpty(hotel.Address) || string.IsNullOrEmpty(hotel.City))
            {
                return BadRequest(new { message = "All fields (name, address, city) are required." });
            }

            var createdHotel = await _hotelService.CreateHotelAsync(hotel);
            return CreatedAtAction(nameof(GetHotelById), new { id = createdHotel.Id }, createdHotel);
        }

        /// <summary>
        /// Updates the status of a hotel (activate/deactivate).
        /// </summary>
        /// <param name="id">Hotel ID.</param>
        /// <param name="isActive">New status.</param>
        /// <returns>Status update message.</returns>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Hotel status updated successfully.")]
        [SwaggerResponse(404, "Hotel not found.")]
        public async Task<ActionResult> UpdateHotelStatus(int id, [FromBody] bool isActive)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = $"Hotel with ID {id} was not found." });
            }

            hotel.IsActive = isActive;
            await _hotelService.UpdateHotelAsync(id, hotel);

            return Ok(new { message = $"Hotel with ID {id} has been {(isActive ? "activated" : "deactivated")}." });
        }

        /// <summary>
        /// Searches for available hotels based on optional filters: city, check-in/check-out dates, and number of guests.
        /// At least one filter is required.
        /// </summary>
        /// <param name="city">Optional. The city where the hotel is located.</param>
        /// <param name="checkIn">Optional. The check-in date (format: YYYY-MM-DD).</param>
        /// <param name="checkOut">Optional. The check-out date (format: YYYY-MM-DD).</param>
        /// <param name="guests">Optional. The number of guests.</param>
        /// <returns>List of available hotels.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<Hotel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SearchHotels(
            [FromQuery] string? city = null,
            [FromQuery] DateTime? checkIn = null,
            [FromQuery] DateTime? checkOut = null,
            [FromQuery] int? guests = null)
        {
            // Validate that at least one filter is provided
            if (string.IsNullOrWhiteSpace(city) && checkIn == null && checkOut == null && guests == null)
            {
                return BadRequest(new { message = "At least one search filter (city, check-in, check-out, guests) must be provided." });
            }

            // Ensure that if dates are provided, check-in is before check-out
            if (checkIn != null && checkOut != null && checkIn >= checkOut)
            {
                return BadRequest(new { message = "Check-out date must be later than check-in date." });
            }

            var hotels = await _hotelService.SearchHotelsAsync(city, checkIn, checkOut, guests);

            if (!hotels.Any())
            {
                return NotFound(new { message = "No available hotels found for the given criteria." });
            }

            return Ok(hotels);
        }
        /// <summary>
        /// Updates an existing hotel.
        /// </summary>
        /// <param name="id">The ID of the hotel to update.</param>
        /// <param name="hotel">The updated hotel details.</param>
        /// <returns>The updated hotel if successful; otherwise, a NotFound or BadRequest response.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] Hotel hotel)
        {
            if (hotel == null)
            {
                return BadRequest(new { message = "Invalid hotel data." });
            }

            var updatedHotel = await _hotelService.UpdateHotelAsync(id, hotel);
            if (updatedHotel == null)
            {
                return NotFound(new { message = $"Hotel with ID {id} not found." });
            }

            return Ok(updatedHotel);
        }


    }
}
