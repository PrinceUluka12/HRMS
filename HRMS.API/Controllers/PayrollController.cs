
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
    /// <summary>
    /// Gets a summary payroll report for a specific period.
    /// </summary>
    [HttpGet("summary-report")]
    [Authorize(Roles = "Payroll.Specialist")]
    
    public async Task<IActionResult> GetPayrollSummaryReport([FromQuery] GetPayrollSummaryReportQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// Gets payroll summary for an individual employee.
    /// </summary>
    [HttpGet("employee-summary")]
    [Authorize(Roles = "Employee")]
    
    public async Task<IActionResult> GetPayrollSummaryByEmployeeId([FromQuery] GetPayrollSummaryByEmployeeIdQuery request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}
