using HRMS.Application.Features.Recruitment.Commands;
using HRMS.Application.Features.Recruitment.Queries;
using HRMS.Application.Interfaces.Services.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RecuritmentsController(IMediator mediator,IAzureBlobStorageService azureBlobStorageService):ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateApplicationCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
    [HttpPost("ChaneApplicationStatus")]
    public async Task<IActionResult> ChangeApplicationStatus([FromForm] ChangeApplicationStatusCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("CreateJobVacancy")]
    public async Task<IActionResult> CreateJobVacancy([FromBody] CreateJobVacancyCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("PublishJobVacancy")]
    public async Task<IActionResult> PublishJobVacancy([FromBody] PublishJobVacancyCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("GetJobVacancyDetails")]
    public async Task<IActionResult> GetJobVacancyDetails([FromQuery] GetJobVacancyDetailsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("GetApplicationByJobVacancyId")]
    public async Task<IActionResult> GetApplicationByJobVacancyId([FromQuery] GetApplicationByJobVacancyIdQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("UploadFile")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var result = await azureBlobStorageService.UploadFileAsync(stream,file.FileName,"application/octet-stream");
        return Ok(result);
    }

    
    
}