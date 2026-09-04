using Airbnb.Application.DTOs.Listing;
using Airbnb.Application.Interfaces;
using Airbnb.Domain.Entities;
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Application.Services;

public class ListingService(AirbnbDbContext db) : IListingService
{
    public async Task<List<ListingReadDto>> GetAllAsync(string? city = null, decimal? maxPrice = null, int? guests = null)
    {
        var query = db.Listings
            .AsNoTracking()
            .Include(x => x.Host)
            .Include(x => x.Images)
            .Include(x => x.Reviews)
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(x => x.City.ToLower().Contains(city.Trim().ToLower()));

        if (maxPrice.HasValue)
            query = query.Where(x => x.PricePerNight <= maxPrice.Value);

        if (guests.HasValue)
            query = query.Where(x => x.MaxGuests >= guests.Value);

        return await query.OrderByDescending(x => x.CreatedAt).Select(x => Map(x)).ToListAsync();
    }

    public async Task<ListingReadDto?> GetByIdAsync(int id)
    {
        var listing = await db.Listings
            .AsNoTracking()
            .Include(x => x.Host)
            .Include(x => x.Images)
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        return listing is null ? null : Map(listing);
    }

    public async Task<ListingReadDto> CreateAsync(int hostId, ListingCreateDto dto)
    {
        var listing = new Listing
        {
            HostId = hostId, Title = dto.Title.Trim(), Description = dto.Description.Trim(),
            PropertyType = dto.PropertyType, City = dto.City.Trim(), Country = dto.Country.Trim(),
            Address = dto.Address.Trim(), PricePerNight = dto.PricePerNight,
            MaxGuests = dto.MaxGuests, Bedrooms = dto.Bedrooms, Beds = dto.Beds, Bathrooms = dto.Bathrooms
        };

        foreach (var (url, index) in dto.ImageUrls.Where(x => !string.IsNullOrWhiteSpace(x)).Take(10).Select((x, i) => (x, i)))
            listing.Images.Add(new ListingImage { ImageUrl = url.Trim(), IsPrimary = index == 0 });

        db.Listings.Add(listing);
        await db.SaveChangesAsync();

        return (await GetByIdAsync(listing.Id))!;
    }

    public async Task<bool> DeleteAsync(int id, int hostId, bool isAdmin)
    {
        var listing = await db.Listings.FirstOrDefaultAsync(x => x.Id == id);
        if (listing is null || (!isAdmin && listing.HostId != hostId))
            return false;

        listing.IsActive = false;
        await db.SaveChangesAsync();
        return true;
    }

    private static ListingReadDto Map(Listing x) => new()
    {
        Id = x.Id, HostId = x.HostId,
        HostName = $"{x.Host.FirstName} {x.Host.LastName}",
        Title = x.Title, Description = x.Description,
        PropertyType = x.PropertyType.ToString(), City = x.City, Country = x.Country,
        Address = x.Address, PricePerNight = x.PricePerNight, MaxGuests = x.MaxGuests,
        Bedrooms = x.Bedrooms, Beds = x.Beds, Bathrooms = x.Bathrooms,
        AverageRating = x.Reviews.Count == 0 ? 0 : Math.Round(x.Reviews.Average(r => r.Rating), 1),
        ReviewCount = x.Reviews.Count,
        ImageUrls = x.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImageUrl).ToList()
    };
}
