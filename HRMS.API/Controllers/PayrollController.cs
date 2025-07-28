
using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Features.Payroll.Queries.GetPayrollSummaryByEmployeeId;
using HRMS.Application.Features.Payroll.Queries.GetPayrollSummaryReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PayrollController(IMediator mediator) : ControllerBase
{
    [HttpGet("GetPayrollSummaryReport")]
    [Authorize(Roles = "Payroll.Specialist")]
    public async Task<IActionResult> GetPayrollSummaryReport([FromQuery] GetPayrollSummaryReportQuery request)
    {
        var result  = await mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("GetPayrollSummaryByEmployeeId")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetPayrollSummaryByEmployeeId([FromQuery] GetPayrollSummaryByEmployeeIdQuery request)
    {
        var result =  await mediator.Send(request);
        return Ok(result);
    }
    // Other endpoints...
}