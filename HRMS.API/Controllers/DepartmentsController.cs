using System.Threading.Tasks;
using HRMS.Application.Features.Departments.Commands.CreateDepartment;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Features.Departments.Queries.GetAllDepartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Employee")]
    public async Task<ActionResult<List<DepartmentDto>>> GetAll()
    {
        var query = new GetAllDepartmentsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result.Id);
    }
}