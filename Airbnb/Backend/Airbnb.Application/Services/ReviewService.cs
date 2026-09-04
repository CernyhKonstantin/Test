using Airbnb.Application.DTOs.Review;
using Airbnb.Application.Interfaces;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Enums;
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Application.Services;

public class ReviewService(AirbnbDbContext db) : IReviewService
{
    public async Task<List<ReviewReadDto>> GetForListingAsync(int listingId) =>
        await db.Reviews.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.ListingId == listingId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ReviewReadDto
            {
                Id = x.Id, ListingId = x.ListingId, UserId = x.UserId,
                UserName = x.User.FirstName + " " + x.User.LastName,
                Rating = x.Rating, Comment = x.Comment, CreatedAt = x.CreatedAt
            }).ToListAsync();

    public async Task<ReviewReadDto> CreateAsync(int userId, int listingId, ReviewCreateDto dto)
    {
        var hasCompletedBooking = await db.Bookings.AnyAsync(x =>
            x.GuestId == userId && x.ListingId == listingId && x.Status == BookingStatus.Completed);

        if (!hasCompletedBooking)
            throw new InvalidOperationException("Only guests with a completed booking can review a listing.");

        if (await db.Reviews.AnyAsync(x => x.UserId == userId && x.ListingId == listingId))
            throw new InvalidOperationException("You have already reviewed this listing.");

        var review = new Review
        {
            UserId = userId, ListingId = listingId, Rating = dto.Rating, Comment = dto.Comment.Trim()
        };

        db.Reviews.Add(review);
        await db.SaveChangesAsync();

        return (await GetForListingAsync(listingId)).First(x => x.Id == review.Id);
    }
}
