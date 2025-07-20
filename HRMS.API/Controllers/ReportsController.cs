using HRMS.Application.Features.Reports.Dtos;
using HRMS.Application.Features.Reports.Queries.GetPayrollSummaryReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize(Roles = "HR.Admin,Payroll.Specialist")]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("payroll-summary")]
    public async Task<ActionResult<PayrollSummaryReportDto>> GetPayrollSummary(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var query = new GetPayrollSummaryReportQuery(startDate, endDate);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /*[HttpGet("employee-turnover")]
    public async Task<ActionResult<EmployeeTurnoverReportDto>> GetEmployeeTurnoverReport(
        [FromQuery] int year)
    {
        var query = new GetEmployeeTurnoverReportQuery(year);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("leave-utilization")]
    public async Task<ActionResult<LeaveUtilizationReportDto>> GetLeaveUtilizationReport(
        [FromQuery] int? year = null,
        [FromQuery] Guid? departmentId = null)
    {
        var query = new GetLeaveUtilizationReportQuery(
            year ?? DateTime.UtcNow.Year,
            departmentId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }*/
}