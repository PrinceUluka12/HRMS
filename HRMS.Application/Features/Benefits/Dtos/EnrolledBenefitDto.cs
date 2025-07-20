namespace HRMS.Application.Features.Benefits.Dtos;

public record EnrolledBenefitDto
{
    public Guid BenefitPlanId { get; init; }
    public string PlanName { get; init; }
    public string PlanType { get; init; } // Medical, Dental, Vision, etc.
    public string CoverageLevel { get; init; } // Employee, Employee+Spouse, Family, etc.
    public decimal EmployeeCostPerPeriod { get; init; }
    public decimal EmployerCostPerPeriod { get; init; }
    public string PeriodFrequency { get; init; } // Monthly, BiWeekly, etc.
    public DateTime CoverageStartDate { get; init; }
    public DateTime? CoverageEndDate { get; init; }
    public bool IsActive { get; init; }
    public IEnumerable<BenefitOptionDto> SelectedOptions { get; init; } = new List<BenefitOptionDto>();
    public IEnumerable<BenefitDependentDto> CoveredDependents { get; init; } = new List<BenefitDependentDto>();
}