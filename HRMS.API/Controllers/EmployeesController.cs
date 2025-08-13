using System;
using System.Threading.Tasks;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Employees.Commands.DeleteEmployee;
using HRMS.Application.Features.Employees.Commands.UpdateEmployee;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Employees.Queries.GetEmployeeByAzureId;
using HRMS.Application.Features.Employees.Queries.GetEmployeeById;
using HRMS.Application.Features.Employees.Queries.GetEmployeeDetail;
using HRMS.Application.Features.Employees.Queries.GetEmployeeList;
using HRMS.Application.Features.Employees.Queries.GetOnboardingDetailsByEmployee;
using HRMS.Application.Features.TimeTracking.Dtos;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new employee.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Gets employee details by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEmployeeDetails(Guid id)
    {
        var result = await mediator.Send(new GetEmployeeByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    /// Gets all employees.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees([FromQuery] GetEmployeeListQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets employee details by Azure AD ID.
    /// </summary>
    [HttpGet("azure")]
    public async Task<IActionResult> GetEmployeeByAzureId([FromQuery] GetEmployeeByAzureIdQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
    /// <summary>
    /// Updates an existing employee. The ID in the route must match the ID in the request body.
    /// Only users with the Admin role can update employees.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(BaseResult<Guid>.Failure(new Error(
                ErrorCode.ModelStateNotValid,
                "Route ID does not match request body ID.",
                nameof(command.Id)
            )));
        }
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an existing employee by ID. Only Admins are allowed.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteEmployeeCommand(id));
        return Ok(result);
    }
    
    
    [HttpGet("onboarding-details")]
    public async Task<IActionResult> GetOnboardingDetails([FromQuery]GetOnboardingDetailsQuery request)
    {
        var  result =  await mediator.Send(request);
        return Ok(result);
    }
}
