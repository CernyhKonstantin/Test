using Airbnb.Application.DTOs.Booking;
using Airbnb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Airbnb.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/bookings")]
public class BookingsController(IBookingService service) : ControllerBase
{
    [HttpGet("mine")]
    public async Task<IActionResult> Mine()
    {
        var userId = GetUserId();
        return Ok(await service.GetMineAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookingCreateDto dto)
    {
        try { return Ok(await service.CreateAsync(GetUserId(), dto)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            return await service.CancelAsync(id, GetUserId())
                ? Ok(new { message = "Booking cancelled." })
                : NotFound(new { message = "Booking not found." });
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
