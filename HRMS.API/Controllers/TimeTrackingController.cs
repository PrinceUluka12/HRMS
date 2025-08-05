using HRMS.Application.Features.TimeTracking.Dtos;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.TimeTrackingAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Employee,Manager,HR")]
public class TimeTrackingController(ITimeTrackingService timeTrackingService) : ControllerBase
{
    [HttpPost("clock-in")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> ClockIn([FromBody] ClockInRequest request)
    {
        var id = await timeTrackingService.ClockInAsync(
            request.EmployeeId,
            request.Date,
            request.Time,
            request.Latitude,
            request.Longitude
        );
        return Ok(id);
    }

    [HttpPost("clock-out")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClockOut([FromBody] ClockOutRequest request)
    {
        await timeTrackingService.ClockOutAsync(
            request.TimeEntryId,
            request.Time,
            request.Latitude,
            request.Longitude
        );
        return NoContent();
    }

    [HttpPost("add-location")]
    public async Task<IActionResult> AddLocation([FromBody] LocationRequest request)
    {
        await timeTrackingService.AddLocationAsync(
            request.TimeEntryId,
            request.ActionType,
            request.Latitude,
            request.Longitude,
            request.Timestamp
        );
        return Ok();
    }

    [HttpPut("complete/{id}")]
    public async Task<IActionResult> Complete([FromRoute] Guid id)
    {
        await timeTrackingService.CompleteEntryAsync(id);
        return Ok();
    }

    [HttpGet("employee/{id}/entries")]
    [ProducesResponseType(typeof(IEnumerable<TimeEntry>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployeeId([FromRoute] Guid id)
    {
        var data = await timeTrackingService.GetByEmployeeIdAsync(id);
        return Ok(data);
    }

    [HttpPost("manual-entry")]
    public async Task<IActionResult> ManualEntryAsync([FromBody] ManualEntryDto request)
    {
        var result = await timeTrackingService.ManualEntryAsync(request);
        return Ok(result);
    }
}
