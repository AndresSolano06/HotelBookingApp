using HotelBooking.Domain.Entities;
using System.Text.Json.Serialization;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string GuestFullName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice { get; set; }

    [JsonIgnore] 
    public Room? Room { get; set; } = null!;
}
