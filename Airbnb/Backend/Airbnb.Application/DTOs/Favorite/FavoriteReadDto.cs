namespace Airbnb.Application.DTOs.Favorite;

public class FavoriteReadDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
