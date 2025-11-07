using HRMS.Application.Features.Notifications.Commands;
using HRMS.Application.Features.Notifications.Dtos;
using HRMS.Application.Features.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class NotificationsController(IMediator _mediator) : ControllerBase
{
    [HttpPost("send")]
    [Authorize]
    public async Task<ActionResult<NotificationDto>> Send([FromBody] CreateNotificationCommand command)
    {
        if (command == null) return BadRequest();
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpGet("employee/{employeeId:guid}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByEmployee([FromRoute] Guid employeeId, [FromQuery] int take = 50, [FromQuery] int skip = 0)
    {
        var q = new GetEmployeeNotificationsQuery { EmployeeId = employeeId, Take = take, Skip = skip };
        var items = await _mediator.Send(q);
        return Ok(items);
    }


    [HttpPost("markread/{notificationId:guid}")]
    [Authorize]
    public async Task<IActionResult> MarkRead([FromRoute] Guid notificationId)
    {
        // Best practice: use authenticated user's employee id, not arbitrary employeeId in body
        var employeeIdClaim = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        if (!Guid.TryParse(employeeIdClaim, out var employeeId)) return Forbid();


        var cmd = new MarkNotificationReadCommand { NotificationId = notificationId, EmployeeId = employeeId };
        await _mediator.Send(cmd);
        return NoContent();
    }


    [HttpGet("unread/count/{employeeId:guid}")]
    [Authorize]
    public async Task<ActionResult<int>> GetUnreadCount([FromRoute] Guid employeeId)
    {
        var q = new GetUnreadCountQuery { EmployeeId = employeeId };
        var count = await _mediator.Send(q);
        return Ok(count);
    }
}