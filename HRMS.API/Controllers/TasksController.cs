using HRMS.Application.Features.Onboarding.Queries.GetAllTasks;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(IMediator mediator) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromQuery] GetAllTasksQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
}