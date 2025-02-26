using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.API.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;

        public RoomController(IRoomService roomService, IHotelService hotelService)
        {
            _roomService = roomService;
            _hotelService = hotelService;
        }

        /// <summary>
        /// Retrieves all rooms for a given hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <returns>List of rooms in the hotel.</returns>
        [HttpGet("{hotelId}")]
        [ProducesResponseType(typeof(IEnumerable<Room>), 200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Rooms retrieved successfully.", typeof(IEnumerable<Room>))]
        [SwaggerResponse(404, "Hotel not found.")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(int hotelId)
        {
            return Ok(await _roomService.GetRoomsByHotelAsync(hotelId));
        }

        /// <summary>
        /// Creates a new room in a hotel.
        /// </summary>
        /// <param name="room">Room details.</param>
        /// <returns>Created room.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Room), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerResponse(201, "Room created successfully.", typeof(Room))]
        [SwaggerResponse(400, "Invalid request. Check input values.")]
        [SwaggerResponse(404, "Hotel not found.")]
        public async Task<ActionResult<Room>> CreateRoom(Room room)
        {
            if (room.HotelId <= 0)
            {
                return BadRequest(new { message = "The hotelId must be greater than 0." });
            }

            var hotel = await _hotelService.GetHotelByIdAsync(room.HotelId);
            if (hotel == null)
            {
                return NotFound(new { message = $"No hotel found with ID {room.HotelId}." });
            }

            if (string.IsNullOrWhiteSpace(room.Type) || string.IsNullOrWhiteSpace(room.Location))
            {
                return BadRequest(new { message = "Room type and location are required." });
            }

            if (room.BasePrice < 0 || room.Taxes < 0)
            {
                return BadRequest(new { message = "Base price and taxes must be positive values." });
            }

            var createdRoom = await _roomService.CreateRoomAsync(room);
            return CreatedAtAction(nameof(GetRoomById), new { id = createdRoom.Id }, createdRoom);
        }

        /// <summary>
        /// Retrieves a room by its ID.
        /// </summary>
        /// <param name="id">Room ID.</param>
        /// <returns>Room details.</returns>
        [HttpGet("byId/{id}")]
        [ProducesResponseType(typeof(Room), 200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Room found successfully.", typeof(Room))]
        [SwaggerResponse(404, "Room not found.")]
        public async Task<ActionResult<Room>> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound(new { message = $"Room with ID {id} was not found." });
            }
            return Ok(room);
        }

        /// <summary>
        /// Updates an existing room.
        /// </summary>
        /// <param name="id">Room ID.</param>
        /// <param name="room">Updated room details.</param>
        /// <returns>Updated room.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Room), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Room updated successfully.", typeof(Room))]
        [SwaggerResponse(400, "Invalid request. Check input values.")]
        [SwaggerResponse(404, "Room not found.")]
        public async Task<ActionResult<Room>> UpdateRoom(int id, Room room)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Room ID must be greater than 0." });
            }

            if (room.HotelId <= 0)
            {
                return BadRequest(new { message = "Hotel ID must be greater than 0." });
            }

            var existingRoom = await _roomService.GetRoomByIdAsync(id);
            if (existingRoom == null)
            {
                return NotFound(new { message = $"Room with ID {id} was not found." });
            }

            var hotel = await _hotelService.GetHotelByIdAsync(room.HotelId);
            if (hotel == null)
            {
                return NotFound(new { message = $"No hotel found with ID {room.HotelId}." });
            }

            if (string.IsNullOrWhiteSpace(room.Type) || string.IsNullOrWhiteSpace(room.Location))
            {
                return BadRequest(new { message = "Room type and location are required." });
            }

            if (room.BasePrice < 0 || room.Taxes < 0)
            {
                return BadRequest(new { message = "Base price and taxes must be positive values." });
            }

            var updatedRoom = await _roomService.UpdateRoomAsync(id, room);
            return Ok(updatedRoom);
        }

        /// <summary>
        /// Activates or deactivates a room.
        /// </summary>
        /// <param name="id">Room ID.</param>
        /// <param name="isActive">New status.</param>
        /// <returns>Status update message.</returns>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerResponse(200, "Room status updated successfully.")]
        [SwaggerResponse(404, "Room not found.")]
        [SwaggerResponse(500, "Error updating room.")]
        public async Task<IActionResult> UpdateRoomStatus(int id, [FromBody] bool isActive)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound(new { message = $"Room with ID {id} was not found." });
            }

            room.IsActive = isActive;
            var updated = await _roomService.UpdateRoomAsync(room);

            if (!updated)
            {
                return StatusCode(500, new { message = "Error updating the room." });
            }

            string statusMessage = isActive ? "activated" : "deactivated";
            return Ok(new { message = $"Room with ID {id} has been {statusMessage}." });
        }
    }
}
