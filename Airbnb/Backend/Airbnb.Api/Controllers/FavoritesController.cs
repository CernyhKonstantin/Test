using Airbnb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Airbnb.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/favorites")]
public class FavoritesController(IFavoriteService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMine() => Ok(await service.GetMineAsync(GetUserId()));

    [HttpPost("{listingId:int}/toggle")]
    public async Task<IActionResult> Toggle(int listingId)
    {
        try { return Ok(new { isFavorite = await service.ToggleAsync(GetUserId(), listingId) }); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
