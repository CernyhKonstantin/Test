using System.ComponentModel.DataAnnotations;

namespace Airbnb.Application.DTOs.Review;

public class ReviewCreateDto
{
    [Range(1, 5)] public int Rating { get; set; }
    [Required, MaxLength(2000)] public string Comment { get; set; } = string.Empty;
}
