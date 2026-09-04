using Airbnb.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Airbnb.Application.DTOs.Listing;

public class ListingCreateDto
{
    [Required, MaxLength(200)] public string Title { get; set; } = string.Empty;
    [Required, MaxLength(4000)] public string Description { get; set; } = string.Empty;
    public PropertyType PropertyType { get; set; }
    [Required] public string City { get; set; } = string.Empty;
    [Required] public string Country { get; set; } = string.Empty;
    [Required] public string Address { get; set; } = string.Empty;
    [Range(1, 100000)] public decimal PricePerNight { get; set; }
    [Range(1, 100)] public int MaxGuests { get; set; }
    [Range(1, 100)] public int Bedrooms { get; set; }
    [Range(1, 100)] public int Beds { get; set; }
    [Range(1, 100)] public int Bathrooms { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
