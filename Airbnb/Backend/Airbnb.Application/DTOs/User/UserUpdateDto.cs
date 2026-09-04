using System.ComponentModel.DataAnnotations;

namespace Airbnb.Application.DTOs.User;

public class UserUpdateDto
{
    [Required, MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
}
