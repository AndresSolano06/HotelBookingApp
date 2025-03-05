using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.API.Controllers
{
    /// <summary>
    /// Controller for managing reservations.
    /// </summary>
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationController"/> class.
        /// </summary>
        public ReservationController(IReservationService reservationService, IRoomService roomService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
        }

        /// <summary>
        /// Retrieves all reservations for a specific room.
        /// </summary>
        [HttpGet("{roomId}")]
        [ProducesResponseType(typeof(IEnumerable<Reservation>), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Get reservations for a room", Description = "Retrieves all reservations for a specific room.")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations(int roomId)
        {
            return Ok(await _reservationService.GetReservationsByRoomAsync(roomId));
        }

        /// <summary>
        /// Retrieves all reservations.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Reservation>), 200)]
        [SwaggerOperation(Summary = "Get all reservations", Description = "Retrieves all reservations.")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
        {
            return Ok(await _reservationService.GetAllReservationsAsync());
        }

        /// <summary>
        /// Retrieves a reservation by its ID.
        /// </summary>
        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(Reservation), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Get reservation by ID", Description = "Retrieves the details of a reservation by its ID.")]
        public async Task<ActionResult<Reservation>> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound(new { message = $"Reservation with ID {id} not found." });
            }
            return Ok(reservation);
        }

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Reservation), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [SwaggerOperation(Summary = "Create a reservation", Description = "Creates a new reservation with guest details and emergency contact.")]
        public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest(new { message = "Reservation data is required." });
            }

            if (reservation.RoomId <= 0)
            {
                return BadRequest(new { message = "RoomId is required and must be greater than 0." });
            }

            var room = await _roomService.GetRoomByIdAsync(reservation.RoomId);
            if (room == null)
            {
                return NotFound(new { message = $"Room with ID {reservation.RoomId} not found." });
            }

            if (room.Capacity < reservation.Guests.Count)
            {
                return BadRequest(new { message = "The selected room does not have enough capacity." });
            }

            if (string.IsNullOrWhiteSpace(reservation.EmergencyContactName) || string.IsNullOrWhiteSpace(reservation.EmergencyContactPhone))
            {
                return BadRequest(new { message = "Emergency contact name and phone are required." });
            }

            var createdReservation = await _reservationService.CreateReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = createdReservation.Id }, createdReservation);
        }

        /// <summary>
        /// Updates an existing reservation.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Reservation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Update a reservation", Description = "Updates the details of an existing reservation.")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest(new { message = "Reservation cannot be null." });
            }

            if (id <= 0)
            {
                return BadRequest(new { message = "The reservation ID must be a valid number." });
            }

            var updatedReservation = await _reservationService.UpdateReservationAsync(id, reservation);

            if (updatedReservation == null)
            {
                return NotFound(new { message = $"Reservation with ID {id} not found or update failed." });
            }

            return Ok(updatedReservation);
        }

        /// <summary>
        /// Cancels an existing reservation.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Cancel a reservation", Description = "Cancels an existing reservation.")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var success = await _reservationService.CancelReservationAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"Reservation with ID {id} was not found." });
            }

            return Ok(new { message = $"Reservation with ID {id} has been successfully canceled." });
        }
    }
}
