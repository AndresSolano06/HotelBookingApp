﻿using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.API.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;

        public ReservationController(IReservationService reservationService, IRoomService roomService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
        }

        /// <summary>
        /// Retrieves all reservations for a specific room.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <returns>List of reservations for the given room.</returns>
        [HttpGet("{roomId}")]
        [ProducesResponseType(typeof(IEnumerable<Reservation>), 200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Reservations retrieved successfully.", typeof(IEnumerable<Reservation>))]
        [SwaggerResponse(404, "Room not found.")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations(int roomId)
        {
            return Ok(await _reservationService.GetReservationsByRoomAsync(roomId));
        }

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="reservation">Reservation details.</param>
        /// <returns>Created reservation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Reservation), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [SwaggerResponse(201, "Reservation created successfully.", typeof(Reservation))]
        [SwaggerResponse(400, "Invalid request. Check input values.")]
        [SwaggerResponse(404, "Room not found.")]
        [SwaggerResponse(409, "Room is already booked for the selected dates.")]
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
        /// Retrieves all reservations.
        /// </summary>
        /// <returns>List of all reservations.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Reservation>), 200)]
        [SwaggerResponse(200, "Reservations retrieved successfully.", typeof(IEnumerable<Reservation>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
        {
            return Ok(await _reservationService.GetAllReservationsAsync());
        }

        /// <summary>
        /// Retrieves a reservation by its ID.
        /// </summary>
        /// <param name="id">Reservation ID.</param>
        /// <returns>Reservation details.</returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(Reservation), 200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Reservation found successfully.", typeof(Reservation))]
        [SwaggerResponse(404, "Reservation not found.")]
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
        /// Updates an existing reservation.
        /// </summary>
        /// <param name="id">Reservation ID.</param>
        /// <param name="reservation">Updated reservation details.</param>
        /// <returns>Updated reservation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Reservation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerResponse(200, "Reservation updated successfully.", typeof(Reservation))]
        [SwaggerResponse(400, "Invalid request. Check input values.")]
        [SwaggerResponse(404, "Reservation not found.")]
        [SwaggerResponse(500, "Error updating reservation.")]
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
        /// Cancels a reservation.
        /// </summary>
        /// <param name="id">Reservation ID.</param>
        /// <returns>Cancellation confirmation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [SwaggerResponse(200, "Reservation canceled successfully.")]
        [SwaggerResponse(404, "Reservation not found.")]
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
