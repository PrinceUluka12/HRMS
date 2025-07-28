using System;
using System.Threading.Tasks;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Employees.Queries.GetEmployeeByAzureId;
using HRMS.Application.Features.Employees.Queries.GetEmployeeById;
using HRMS.Application.Features.Employees.Queries.GetEmployeeDetail;
using HRMS.Application.Features.Employees.Queries.GetEmployeeList;
using HRMS.Application.Features.TimeTracking.Dtos;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

//[Authorize]
[ApiController]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class EmployeesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    //[Authorize(Roles = "HR.Admin")]
    [Route("api/[controller]")]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeCommand command)
    {
        var result = await mediator.Send(command);
        return Ok (result);
    }

    [HttpGet("api/[controller]/{Id}")]

    public async Task<ActionResult<EmployeeDetailDto>> GetEmployeeDetails([FromRoute]GetEmployeeByIdQuery request)
    {
       
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpGet]
    [Route("api/[controller]")]
    public async Task<ActionResult<List<EmployeeListDto>>> GetAllEmployees()
    {
        var query = new GetEmployeeListQuery();
        
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("by-azure-id")]
    public async Task<IActionResult> GetEmployeeByAzureId([FromQuery] GetEmployeeByAzureIdQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    
    // Other endpoints...
}