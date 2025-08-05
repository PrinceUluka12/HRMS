using HRMS.Application.Features.Notifications.GetAllNotifications;
using HRMS.Application.Features.Notifications.GetAllNotificationsByEmployeeId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllNotificationsQuery  request)
    {
        var result = await mediator.Send(request);

        return Ok(result);
    }
    
    [HttpGet("GetAllNotificationsByEmployeeIdQuery")]
    public async Task<IActionResult> GetByEmployeeId([FromQuery] GetAllNotificationsByEmployeeIdQuery  request)
    {
        var result = await mediator.Send(request);

        return Ok(result);
    }
}