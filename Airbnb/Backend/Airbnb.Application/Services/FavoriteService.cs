using Airbnb.Application.DTOs.Favorite;
using Airbnb.Application.Interfaces;
using Airbnb.Domain.Entities;
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Application.Services;

public class FavoriteService(AirbnbDbContext db) : IFavoriteService
{
    public async Task<List<FavoriteReadDto>> GetMineAsync(int userId) =>
        await db.Favorites.AsNoTracking()
            .Include(x => x.Listing).ThenInclude(x => x.Images)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new FavoriteReadDto
            {
                Id = x.Id, ListingId = x.ListingId, Title = x.Listing.Title,
                PricePerNight = x.Listing.PricePerNight, City = x.Listing.City,
                Country = x.Listing.Country,
                ImageUrl = x.Listing.Images.OrderByDescending(i => i.IsPrimary)
                    .Select(i => i.ImageUrl).FirstOrDefault() ?? ""
            }).ToListAsync();

    public async Task<bool> ToggleAsync(int userId, int listingId)
    {
        var existing = await db.Favorites.FirstOrDefaultAsync(x => x.UserId == userId && x.ListingId == listingId);
        if (existing is not null)
        {
            db.Favorites.Remove(existing);
            await db.SaveChangesAsync();
            return false;
        }

        if (!await db.Listings.AnyAsync(x => x.Id == listingId && x.IsActive))
            throw new KeyNotFoundException("Listing not found.");

        db.Favorites.Add(new Favorite { UserId = userId, ListingId = listingId });
        await db.SaveChangesAsync();
        return true;
    }
}
