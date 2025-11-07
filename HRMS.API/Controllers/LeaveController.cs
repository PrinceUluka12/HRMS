using HRMS.Application.Features.Leave.Commands.ApproveLeave;
using HRMS.Application.Features.Leave.Commands.CancelLeave;
using HRMS.Application.Features.Leave.Commands.RejectLeave;
using HRMS.Application.Features.Leave.Commands.RequestLeave;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveBalance;
using HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveRequests;
using HRMS.Application.Features.Leave.Queries.GetPendingLeaveRequests;
using HRMS.Application.Features.Leave.Queries.GetTeamLeaveRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaveController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Submit a leave request.
    /// </summary>
    [HttpPost("request")]
    public async Task<IActionResult> RequestLeave([FromBody] RequestLeaveCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// cancel a leave request.
    /// </summary>
    [HttpPost("cancel-request")]
    public async Task<IActionResult> CancelLeave([FromBody] CancelLeaveRequestCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Reject a leave request.
    /// </summary>
    [HttpPost("reject-request")]
    public async Task<IActionResult> RejectLeave([FromBody] RejectLeaveRequestCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Approve a pending leave request.
    /// </summary>
    [HttpPost("{leaveRequestId:guid}/approve")]
    [Authorize(Roles = "HR,Department.Manager")]
    public async Task<IActionResult> ApproveLeave(Guid leaveRequestId, Guid ManagerId, string Comment)
    {
        var command = new ApproveLeaveCommand(leaveRequestId, ManagerId, Comment);
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Get leave requests for an employee. 
    /// </summary>
    [HttpGet("employee/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeLeaveRequests([FromRoute] Guid employeeId)
    {
        var result = await mediator.Send(new GetEmployeeLeaveRequestsQuery(employeeId));
        return Ok(result);
    }

    /// <summary>
    /// Get leave balance for an employee. 
    /// </summary>
    [HttpGet("balance")]
    public async Task<IActionResult> GetEmployeeLeaveBalance([FromQuery] GetEmployeeLeaveBalanceQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }


    /// <summary>
    /// Get leave requests from a manager's team.
    /// </summary>
    [HttpGet("team")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetTeamLeaveRequests([FromQuery] GetTeamLeaveRequestsQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// Get all pending leave requests.
    /// </summary>
    [HttpGet("pending")]
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> GetPendingLeaveRequests([FromQuery] GetPendingLeaveRequestsQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}
