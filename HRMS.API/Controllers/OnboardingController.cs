using HRMS.Application.Features.Onboarding.Queries.GetAllOnboarding;
using HRMS.Application.Features.Onboarding.Queries.GetOnboardingByEmployeeId;
using HRMS.Application.Interfaces.Services.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OnboardingController(IMediator mediator, IOnboardingService onboardingService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]GetAllOnboardingQuery request)
    {
        var  result =  await mediator.Send(request);
        return Ok(result);
    }
    
    
    [HttpGet("GetOnboardingByEmployeeIdQuery")]
    public async Task<IActionResult> GetAll([FromQuery]GetOnboardingByEmployeeIdQuery request)
    {
        var  result =  await mediator.Send(request);
        return Ok(result);
    }
        
    
    [HttpPost]
    public async Task<IActionResult> ManualStart(Guid  Id)
    {
        var  result =  await onboardingService.ManualStartOnboarding(Id,CancellationToken.None);
        return Ok(result);
    }
}