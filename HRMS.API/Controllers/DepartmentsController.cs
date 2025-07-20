using System.Threading.Tasks;
using HRMS.Application.Features.Departments.Commands.CreateDepartment;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Features.Departments.Queries.GetAllDepartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize(Roles = "Admin,Department.Manager")]
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DepartmentDto>>> GetAll()
    {
        var query = new GetAllDepartmentsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Id);
    }
}