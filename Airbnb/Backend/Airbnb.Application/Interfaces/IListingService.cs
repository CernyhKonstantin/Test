using Airbnb.Application.DTOs.Listing;

namespace Airbnb.Application.Interfaces;

public interface IListingService
{
    Task<List<ListingReadDto>> GetAllAsync(string? city = null, decimal? maxPrice = null, int? guests = null);
    Task<ListingReadDto?> GetByIdAsync(int id);
    Task<ListingReadDto> CreateAsync(int hostId, ListingCreateDto dto);
    Task<bool> DeleteAsync(int id, int hostId, bool isAdmin);
}
