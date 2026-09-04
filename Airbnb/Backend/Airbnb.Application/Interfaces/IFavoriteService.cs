using Airbnb.Application.DTOs.Favorite;

namespace Airbnb.Application.Interfaces;

public interface IFavoriteService
{
    Task<List<FavoriteReadDto>> GetMineAsync(int userId);
    Task<bool> ToggleAsync(int userId, int listingId);
}
