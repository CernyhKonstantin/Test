using Airbnb.Application.DTOs.Review;

namespace Airbnb.Application.Interfaces;

public interface IReviewService
{
    Task<List<ReviewReadDto>> GetForListingAsync(int listingId);
    Task<ReviewReadDto> CreateAsync(int userId, int listingId, ReviewCreateDto dto);
}
