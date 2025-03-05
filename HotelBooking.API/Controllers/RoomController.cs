using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        /// Retrieves all rooms for a specific hotel.
        /// </summary>
        [HttpGet("{hotelId}")]
        [ProducesResponseType(typeof(IEnumerable<Room>), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Get rooms by hotel", Description = "Retrieves all rooms for a given hotel.")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(int hotelId)
        {
            var rooms = await _roomService.GetRoomsByHotelAsync(hotelId);
            if (rooms == null || !rooms.Any())
            {
                return NotFound(new { message = "No rooms found for this hotel." });
            }
            return Ok(rooms);
        }

        /// <summary>
        /// Retrieves a room by its ID.
        /// </summary>
        [HttpGet("byId/{id}")]
        [ProducesResponseType(typeof(Room), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Get room by ID", Description = "Retrieves details of a specific room.")]
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
        /// Creates a new room in a hotel.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(Room), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Create a new room", Description = "Adds a new room to a specified hotel.")]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] Room room)
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
        /// Updates an existing room.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(Room), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Update a room", Description = "Updates the details of an existing room.")]
        public async Task<ActionResult<Room>> UpdateRoom(int id, [FromBody] Room room)
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
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Activate/Deactivate a room", Description = "Updates the status of a room (active/inactive).")]
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
