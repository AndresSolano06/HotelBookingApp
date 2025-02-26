using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{roomId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations(int roomId)
        {
            return Ok(await _reservationService.GetReservationsByRoomAsync(roomId));
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest("Reservation data is required.");
            }

            if (reservation.RoomId <= 0)
            {
                return BadRequest("RoomId is required and must be greater than 0.");
            }

            // Buscar la habitación antes de crear la reserva
            var room = await _roomService.GetRoomByIdAsync(reservation.RoomId);
            if (room == null)
            {
                return NotFound($"Room with Id {reservation.RoomId} not found.");
            }

            reservation.Room = room;

            var createdReservation = await _reservationService.CreateReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservations), new { roomId = reservation.RoomId }, createdReservation);
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
        {
            return Ok(await _reservationService.GetAllReservationsAsync());
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Reservation>> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound($"Reservation with Id {id} not found.");
            }
            return Ok(reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            var updatedReservation = await _reservationService.UpdateReservationAsync(id, reservation);
            if (updatedReservation == null)
            {
                return NotFound($"Reservation with Id {id} not found.");
            }
            return Ok(updatedReservation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var success = await _reservationService.CancelReservationAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"Reservation with ID {id} not found." });
            }

            return Ok(new { message = $"Reservation with ID {id} has been successfully canceled." });
        }

    }
}
