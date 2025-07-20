using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Leave.Dtos;

public record LeaveBalanceDto
{
    public LeaveType LeaveType { get; init; }
    public decimal AnnualEntitlement { get; init; }
    public decimal AccruedYTD { get; init; }
    public decimal TakenYTD { get; init; }
    public decimal PendingApproval { get; init; }
    public decimal AvailableBalance { get; init; }
    public decimal CarryoverFromPreviousYear { get; init; }
    public DateTime FiscalYearStart { get; init; }
    public DateTime FiscalYearEnd { get; init; }

    public decimal PercentageUsed => AnnualEntitlement > 0 
        ? (TakenYTD / AnnualEntitlement) * 100 
        : 0;

    public LeaveBalanceDto(
        LeaveType leaveType,
        decimal annualEntitlement,
        decimal accruedYTD,
        decimal takenYTD,
        decimal pendingApproval,
        decimal carryoverFromPreviousYear,
        DateTime fiscalYearStart,
        DateTime fiscalYearEnd)
    {
        LeaveType = leaveType;
        AnnualEntitlement = annualEntitlement;
        AccruedYTD = accruedYTD;
        TakenYTD = takenYTD;
        PendingApproval = pendingApproval;
        CarryoverFromPreviousYear = carryoverFromPreviousYear;
        FiscalYearStart = fiscalYearStart;
        FiscalYearEnd = fiscalYearEnd;
        
        AvailableBalance = (annualEntitlement + carryoverFromPreviousYear) - 
                           (takenYTD + pendingApproval);
    }
}

public record EmployeeLeaveSummaryDto
{
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; }
    public IEnumerable<LeaveBalanceDto> Balances { get; init; }
    public decimal TotalAvailableBalance => Balances.Sum(b => b.AvailableBalance);
    public decimal TotalUsedYTD => Balances.Sum(b => b.TakenYTD);
    public DateTime AsOfDate  { get; init; }
}