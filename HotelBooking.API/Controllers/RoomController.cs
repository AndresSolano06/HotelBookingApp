using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.API.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("{hotelId}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(int hotelId)
        {
            return Ok(await _roomService.GetRoomsByHotelAsync(hotelId));
        }

        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom(Room room)
        {
            var createdRoom = await _roomService.CreateRoomAsync(room);
            return CreatedAtAction(nameof(GetRooms), new { hotelId = room.HotelId }, createdRoom);
        }
    }
}
