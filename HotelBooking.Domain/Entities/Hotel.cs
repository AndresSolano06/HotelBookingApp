using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "The City field is required.")]
        public string City { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public List<Room> Rooms { get; set; } = new();
    }
}
