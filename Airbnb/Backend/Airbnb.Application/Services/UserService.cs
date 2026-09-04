using Airbnb.Application.DTOs.Auth;
using Airbnb.Application.DTOs.User;
using Airbnb.Application.Interfaces;
using Airbnb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.Application.Services;

public class UserService(AirbnbDbContext db) : IUserService
{
    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var u = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return u is null ? null : Map(u);
    }

    public async Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto)
    {
        var u = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (u is null) return null;

        u.FirstName = dto.FirstName.Trim();
        u.LastName = dto.LastName.Trim();
        u.PhoneNumber = dto.PhoneNumber?.Trim();
        u.ProfileImageUrl = dto.ProfileImageUrl?.Trim();
        await db.SaveChangesAsync();

        return Map(u);
    }

    private static UserDto Map(Airbnb.Domain.Entities.User u) => new()
    {
        Id = u.Id, FirstName = u.FirstName, LastName = u.LastName,
        Email = u.Email, PhoneNumber = u.PhoneNumber,
        ProfileImageUrl = u.ProfileImageUrl, Role = u.Role.ToString()
    };
}
