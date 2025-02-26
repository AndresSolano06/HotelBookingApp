

namespace HotelBooking.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<Room> Rooms { get; set; } = new();
    }
}
