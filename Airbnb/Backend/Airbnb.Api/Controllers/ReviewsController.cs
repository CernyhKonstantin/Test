using Airbnb.Application.DTOs.Review;
using Airbnb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Airbnb.Api.Controllers;

[ApiController]
[Route("api/listings/{listingId:int}/reviews")]
public class ReviewsController(IReviewService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(int listingId) => Ok(await service.GetForListingAsync(listingId));

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(int listingId, ReviewCreateDto dto)
    {
        try { return Ok(await service.CreateAsync(GetUserId(), listingId, dto)); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
