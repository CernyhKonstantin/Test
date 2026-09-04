using System.ComponentModel.DataAnnotations;

namespace Airbnb.Application.DTOs.Booking;

public class BookingCreateDto
{
    [Range(1, int.MaxValue)] public int ListingId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    [Range(1, 100)] public int Guests { get; set; }
}
