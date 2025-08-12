using HRMS.Application.Features.BuddyPair.Commands.CreateBuddyPair;
using HRMS.Application.Features.BuddyPair.Queries.GetBuddyPairByEmployee;
using HRMS.Application.Features.BuddyPairs.Queries.GetCheckinsByPairId;
using HRMS.Application.Features.BuddyPairs.Queries.GetPendingCheckins;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuddyPairController(IMediator mediator) : ControllerBase
{
    [HttpGet("employee")]
    public async Task<IActionResult> GetBuddyPairByEmployee([FromQuery]GetBuddyPairByEmployeeQuery request)
    {
        var  result =  await mediator.Send(request);
        return Ok(result);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateBuddyPair(CreateBuddyPairCommand request)
    {
        var  result =  await mediator.Send(request);
        return Ok(result);
    }
    [HttpGet("checkins")]
    public async Task<IActionResult> GetCheckinsByPairId([FromQuery]GetCheckinsByPairIdQuery request)
    {
        var  result =  await mediator.Send(request);
        return Ok(result);
    }
    // [HttpPost("create-checkin")]
    // public async Task<IActionResult> CreateBuddyPair(CreateCheckinCommand request)
    // {
    //     var  result =  await mediator.Send(request);
    //     return Ok(result);
    // }
    [HttpGet("pending-checkins")]
    public async Task<IActionResult> GetPendingCheckins([FromQuery]GetPendingCheckinsQuery request)
    {
        var  result =  await mediator.Send(request);
        return Ok(result);
    }
    
}