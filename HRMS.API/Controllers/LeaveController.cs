using HRMS.Application.Features.Leave.Commands.RequestLeave;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveRequests;
using HRMS.Application.Features.Leave.Queries.GetTeamLeaveRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("request")]
    public async Task<ActionResult<LeaveRequestDto>> RequestLeave(RequestLeaveCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /*[HttpPost("approve/{id}")]
    [Authorize(Roles = "HR.Admin,Department.Manager")]
    public async Task<ActionResult> ApproveLeave(Guid id)
    {
        var command = new ApproveLeaveCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }*/

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetEmployeeLeaveRequests(Guid employeeId)
    {
        var query = new GetEmployeeLeaveRequestsQuery(employeeId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("GetTeamLeaveRequests")]
    public async Task<IActionResult> GetTeamLeaveRequests([FromQuery] GetTeamLeaveRequestsQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}