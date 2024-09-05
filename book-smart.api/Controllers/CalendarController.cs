using BookSmart.Api.Models.Requests;
using BookSmart.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookSmart.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;

    public CalendarController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    [HttpPost("query")]
    public async Task<IActionResult> GetAvailableSlots([FromBody] CalendarQueryRequestModel request)
    {
        var availableSlots = await _calendarService.GetAvailableSlotsAsync(request);
        return Ok(availableSlots);
    }
}