using Airbnb.Application.DTOs.Booking;

namespace Airbnb.Application.Interfaces;

public interface IBookingService
{
    Task<BookingReadDto> CreateAsync(int guestId, BookingCreateDto dto);
    Task<List<BookingReadDto>> GetMineAsync(int userId);
    Task<bool> CancelAsync(int bookingId, int userId);
}
