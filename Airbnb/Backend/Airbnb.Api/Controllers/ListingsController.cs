using Airbnb.Application.DTOs.Listing;
using Airbnb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Airbnb.Api.Controllers;

[ApiController]
[Route("api/listings")]
public class ListingsController(IListingService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? city, [FromQuery] decimal? maxPrice, [FromQuery] int? guests) =>
        Ok(await service.GetAllAsync(city, maxPrice, guests));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);
        return result is null ? NotFound(new { message = "Listing not found." }) : Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(ListingCreateDto dto)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return Unauthorized();

        var result = await service.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        return await service.DeleteAsync(id, userId, isAdmin)
            ? NoContent()
            : NotFound(new { message = "Listing not found or you do not have permission." });
    }
}
