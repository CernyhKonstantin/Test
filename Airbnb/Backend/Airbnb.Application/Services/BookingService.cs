using Airbnb.Application.DTOs.Booking;
using Airbnb.Application.Interfaces;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Enums;
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Application.Services;

public class BookingService(AirbnbDbContext db) : IBookingService
{
    public async Task<BookingReadDto> CreateAsync(int guestId, BookingCreateDto dto)
    {
        if (dto.CheckIn.Date < DateTime.UtcNow.Date)
            throw new InvalidOperationException("Check-in cannot be in the past.");

        if (dto.CheckOut.Date <= dto.CheckIn.Date)
            throw new InvalidOperationException("Check-out must be after check-in.");

        var listing = await db.Listings.Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == dto.ListingId && x.IsActive);

        if (listing is null)
            throw new KeyNotFoundException("Listing not found.");

        if (dto.Guests > listing.MaxGuests)
            throw new InvalidOperationException("The number of guests exceeds the listing capacity.");

        var conflict = await db.Bookings.AnyAsync(x =>
            x.ListingId == dto.ListingId &&
            x.Status != BookingStatus.Cancelled &&
            dto.CheckIn.Date < x.CheckOut.Date &&
            dto.CheckOut.Date > x.CheckIn.Date);

        if (conflict)
            throw new InvalidOperationException("The listing is not available for the selected dates.");

        var nights = (dto.CheckOut.Date - dto.CheckIn.Date).Days;
        var booking = new Booking
        {
            ListingId = dto.ListingId, GuestId = guestId,
            CheckIn = dto.CheckIn.Date, CheckOut = dto.CheckOut.Date,
            Guests = dto.Guests, TotalPrice = nights * listing.PricePerNight,
            Status = BookingStatus.Confirmed
        };

        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        return (await GetMineAsync(guestId)).First(x => x.Id == booking.Id);
    }

    public async Task<List<BookingReadDto>> GetMineAsync(int userId)
    {
        return await db.Bookings.AsNoTracking()
            .Include(x => x.Listing).ThenInclude(x => x.Images)
            .Where(x => x.GuestId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new BookingReadDto
            {
                Id = x.Id, ListingId = x.ListingId, ListingTitle = x.Listing.Title,
                MainImageUrl = x.Listing.Images.Where(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault()
                    ?? x.Listing.Images.Select(i => i.ImageUrl).FirstOrDefault() ?? "",
                CheckIn = x.CheckIn, CheckOut = x.CheckOut, Guests = x.Guests,
                TotalPrice = x.TotalPrice, Status = x.Status.ToString()
            }).ToListAsync();
    }

    public async Task<bool> CancelAsync(int bookingId, int userId)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId && x.GuestId == userId);
        if (booking is null) return false;

        if (booking.Status == BookingStatus.Completed)
            throw new InvalidOperationException("Completed bookings cannot be cancelled.");

        booking.Status = BookingStatus.Cancelled;
        await db.SaveChangesAsync();
        return true;
    }
}
