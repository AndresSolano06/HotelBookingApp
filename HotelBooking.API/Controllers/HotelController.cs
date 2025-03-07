using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.API.Controllers
{
    /// <summary>
    /// Controller for managing hotel-related operations.
    /// </summary>
    [Route("api/hotel")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelController"/> class.
        /// </summary>
        /// <param name="hotelService">Service for hotel operations.</param>
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Retrieves all hotels.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Hotel>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Get all hotels", Description = "Retrieves a list of all hotels.")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels([FromQuery] bool includeInactive = false)
        {
            var hotels = await _hotelService.GetAllHotelsAsync(includeInactive);
            return Ok(hotels);
        }

        /// <summary>
        /// Retrieves a hotel by its ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(404)]
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
        /// Creates a new hotel (Admin Only).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(Hotel), 201)]
        [ProducesResponseType(400)]
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
        /// Updates an existing hotel (Admin Only).
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Updates the status of a hotel (Activate/Deactivate) (Admin Only).
        /// </summary>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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
        /// Deletes a hotel by its ID (Admin Only).
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                var success = await _hotelService.DeleteHotelAsync(id);

                if (!success)
                {
                    return NotFound(new { message = $"Hotel with ID {id} was not found." });
                }

                return Ok(new { message = $"Hotel with ID {id} has been successfully deleted." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message }); // Maneja el caso de habitaciones activas
            }
        }


        /// <summary>
        /// Searches for available hotels based on optional filters.
        /// At least one filter is required.
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<Hotel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SearchHotels(
        [FromQuery] string? city = null,
        [FromQuery] DateTime? CheckInDate = null,
        [FromQuery] DateTime? CheckOutDate = null,
        [FromQuery] int? guests = null)
        {
            try
            {
                var hotels = await _hotelService.SearchHotelsAsync(city, CheckInDate, CheckOutDate, guests);

                if (!hotels.Any())
                {
                    return NotFound(new { message = "No available hotels found for the given criteria." });
                }

                return Ok(hotels);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
