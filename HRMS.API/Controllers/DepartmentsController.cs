using System;
using System.Threading.Tasks;
using HRMS.Application.Features.Departments.Commands.CreateDepartment;
using HRMS.Application.Features.Departments.Commands.UpdateDepartment;
using HRMS.Application.Features.Departments.Commands.DeleteDepartment;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Features.Departments.Queries.GetAllDepartments;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves all departments.
    /// </summary>
    /// <returns>A list of departments.</returns>
    [HttpGet]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDepartmentsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new department.
    /// </summary>
    /// <param name="command">The department creation request.</param>
    /// <returns>The created department.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing department. Requires Admin role. The ID in the route must match the body.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentCommand command)
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
    /// Deletes an existing department. Requires Admin role.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteDepartmentCommand(id));
        return Ok(result);
    }
}
