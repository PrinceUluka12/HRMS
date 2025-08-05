using HRMS.Application.Features.Reports.Dtos;
using HRMS.Application.Features.Reports.Queries.GetPayrollSummaryReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize(Roles = "HR.Admin,Payroll.Specialist")]
[ApiController]
[Route("api/[controller]")]
public class ReportsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves a summary of payroll between a date range.
    /// </summary>
    [HttpGet("payroll-summary")]
    public async Task<IActionResult> GetPayrollSummary([FromQuery] GetPayrollSummaryReportQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /*
    /// <summary>
    /// Retrieves the employee turnover report for a given year.
    /// </summary>
    [HttpGet("employee-turnover")]
    [ProducesResponseType(typeof(EmployeeTurnoverReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeTurnoverReport([FromQuery] int year)
    {
        var query = new GetEmployeeTurnoverReportQuery(year);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves leave utilization report optionally filtered by year and department.
    /// </summary>
    [HttpGet("leave-utilization")]
    [ProducesResponseType(typeof(LeaveUtilizationReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLeaveUtilizationReport([FromQuery] int? year = null, [FromQuery] Guid? departmentId = null)
    {
        var query = new GetLeaveUtilizationReportQuery(year ?? DateTime.UtcNow.Year, departmentId);
        var result = await mediator.Send(query);
        return Ok(result);
    }
    */
}