using Airbnb.Application.DTOs.Auth;
using Airbnb.Application.DTOs.User;

namespace Airbnb.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto);
}
