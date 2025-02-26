using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            return Ok(await _hotelService.GetAllHotelsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotelById(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = $"El hotel con ID {id} no fue encontrado." });
            }
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> CreateHotel(Hotel hotel)
        {
            var createdHotel = await _hotelService.CreateHotelAsync(hotel);
            return CreatedAtAction(nameof(GetHotelById), new { id = createdHotel.Id }, createdHotel);
        }
    }
}
