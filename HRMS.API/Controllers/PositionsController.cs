using HRMS.Application.Features.Positions.Commands.CreatePosition;
using HRMS.Application.Features.Positions.Dtos;
using HRMS.Application.Features.Positions.Queries.GetAllPositions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all positions.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin, HR.Manager")]
    
    public async Task<IActionResult> GetAll([FromQuery] GetAllPositionsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a new position.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "HR.Admin")]
    
    public async Task<IActionResult> Create([FromBody] CreatePositionCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}