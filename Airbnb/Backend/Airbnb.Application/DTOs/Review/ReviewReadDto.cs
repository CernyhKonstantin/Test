namespace Airbnb.Application.DTOs.Review;

public class ReviewReadDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
