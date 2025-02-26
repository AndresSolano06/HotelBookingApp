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
    }
}
