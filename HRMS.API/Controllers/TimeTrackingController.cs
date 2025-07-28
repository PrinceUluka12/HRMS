using HRMS.Application.Features.TimeTracking.Dtos;
using HRMS.Application.Interfaces.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Employee,Manager,HR")]
public class TimeTrackingController(ITimeTrackingService _timeTrackingService) : ControllerBase
{
    [HttpPost("clock-in")]
    public async Task<IActionResult> ClockIn([FromBody] ClockInRequest request)
    {
        var id = await _timeTrackingService.ClockInAsync(
            request.EmployeeId,
            request.Date,
            request.Time,
            request.Latitude,
            request.Longitude
        );

        return Ok(id);
    }
    
    [HttpPost("clock-out")]
    public async Task<IActionResult> ClockOut([FromBody] ClockOutRequest request)
    {
        await _timeTrackingService.ClockOutAsync(
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
        await _timeTrackingService.AddLocationAsync(
            request.TimeEntryId,
            request.ActionType,
            request.Latitude,
            request.Longitude,
            request.Timestamp
        );

        return Ok();
    }
    
    [HttpPost("complete/{id}")]
    public async Task<IActionResult> Complete(Guid id)
    {
        await _timeTrackingService.CompleteEntryAsync(id);
        return Ok();
    }

   
    [HttpGet("entries/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
       var data = await _timeTrackingService.GetByEmployeeIdAsync(id);
        return Ok(data);
    }
    
    [HttpPost("manual-entry")]
    public async Task<IActionResult> ManualEntryAsync([FromBody]ManualEntryDto request)
    {
        var result = await _timeTrackingService.ManualEntryAsync(request);
        return Ok(result);
    }
}