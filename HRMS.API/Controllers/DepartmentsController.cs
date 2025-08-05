using System.Threading.Tasks;
using HRMS.Application.Features.Departments.Commands.CreateDepartment;
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
    [Authorize(Roles = "Employee")]
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
}
