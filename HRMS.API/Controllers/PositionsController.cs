using HRMS.Application.Features.Positions.Commands.CreatePosition;
using HRMS.Application.Features.Positions.Dtos;
using HRMS.Application.Features.Positions.Queries.GetAllPositions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

//[Authorize(Roles = "HR.Admin")]
[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PositionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<PositionDto>>> GetAll()
    {
        var query = new GetAllPositionsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PositionDto>> Create(CreatePositionCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }
}