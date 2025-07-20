using System;
using System.Threading.Tasks;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Employees.Queries.GetEmployeeDetail;
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
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDetailDto>> Get(Guid id)
    {
        var query = new GetEmployeeDetailQuery(id);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    // Other endpoints...
}