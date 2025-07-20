using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Benefits.Dtos;

public record BenefitsSummaryDto
{
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; }
    public DateTime AsOfDate { get; init; }
    public string EmploymentStatus { get; init; }
    public bool IsBenefitsEligible { get; init; }
    public DateTime EligibilityDate { get; init; }
    public DateTime? NextOpenEnrollmentDate { get; init; }
    
    // Cost Breakdown
    public decimal TotalAnnualEmployeeCost { get; init; }
    public decimal TotalAnnualEmployerCost { get; init; }
    public decimal TotalAnnualCost => TotalAnnualEmployeeCost + TotalAnnualEmployerCost;
    public decimal EmployeeCostPerPayPeriod { get; init; }
    
    // Coverage Summary
    public int TotalCoveredLives { get; init; } // Employee + dependents
    public IEnumerable<BenefitCoverageSummaryDto> CoverageSummary { get; init; } = new List<BenefitCoverageSummaryDto>();
    
    // Plan Details
    public IEnumerable<ActiveBenefitPlanDto> ActivePlans { get; init; } = new List<ActiveBenefitPlanDto>();
    public IEnumerable<AvailableBenefitPlanDto> AvailablePlans { get; init; } = new List<AvailableBenefitPlanDto>();
    
    // Life Events
    public IEnumerable<QualifyingLifeEventDto> UpcomingLifeEvents { get; init; } = new List<QualifyingLifeEventDto>();
    public bool HasPendingLifeEvent { get; init; }
    
    // Documents
    public IEnumerable<BenefitsDocumentStatusDto> DocumentStatuses { get; init; } = new List<BenefitsDocumentStatusDto>();
    
    // Calculated Properties
    public decimal EmployeeCostPercentage => TotalAnnualCost > 0 
        ? (TotalAnnualEmployeeCost / TotalAnnualCost) * 100 
        : 0;
        
    public string CostSharingDescription => 
        $"Employee covers {EmployeeCostPercentage:N1}% of total benefits cost";
    
    // Helper Methods
    public bool HasMedicalCoverage => 
        ActivePlans.Any(p => p.PlanType == BenefitPlanType.Medical);
        
    public bool HasRetirementPlan => 
        ActivePlans.Any(p => p.PlanType == BenefitPlanType.Retirement);
        
    public IEnumerable<ActiveBenefitPlanDto> GetPlansByType(BenefitPlanType type) => 
        ActivePlans.Where(p => p.PlanType == type);
}

// Supporting DTOs


public record ActiveBenefitPlanDto
{
    public Guid EnrollmentId { get; init; }
    public Guid PlanId { get; init; }
    public string PlanName { get; init; }
    public BenefitPlanType PlanType { get; init; }
    public string PlanTypeName => PlanType.ToString();
    public string CarrierName { get; init; }
    public DateTime EffectiveDate { get; init; }
    public DateTime? TerminationDate { get; init; }
    public decimal EmployeeCost { get; init; }
    public decimal EmployerCost { get; init; }
    public string BillingFrequency { get; init; } // "Monthly", "BiWeekly"
    public IEnumerable<CoveredIndividualDto> CoveredIndividuals { get; init; } = new List<CoveredIndividualDto>();
    public IEnumerable<PlanFeatureDto> KeyFeatures { get; init; } = new List<PlanFeatureDto>();
    public string SummaryDocumentUrl { get; init; }
}

public record AvailableBenefitPlanDto
{
    public Guid PlanId { get; init; }
    public string PlanName { get; init; }
    public BenefitPlanType PlanType { get; init; }
    public string ShortDescription { get; init; }
    public decimal EstimatedEmployeeCost { get; init; }
    public decimal EstimatedEmployerCost { get; init; }
    public bool IsNew { get; init; }
    public DateTime EnrollmentDeadline { get; init; }
    public IEnumerable<string> CoverageOptions { get; init; } = new List<string>(); // "Employee", "Employee+Spouse", etc.
    public string ComparisonChartUrl { get; init; }
}

public record CoveredIndividualDto
{
    public Guid IndividualId { get; init; }
    public string Name { get; init; }
    public string Relationship { get; init; } // "Employee", "Spouse", "Child"
    public DateTime DateOfBirth { get; init; }
    public bool IsStudent { get; init; }
    public string CoverageStatus { get; init; } // "Active", "Pending Verification"
}

public record PlanFeatureDto
{
    public string FeatureName { get; init; }
    public string Description { get; init; }
    public string Value { get; init; } // e.g., "$20 copay", "100% coverage"
    public bool IsHighlighted { get; init; }
}

public record QualifyingLifeEventDto
{
    public Guid EventId { get; init; }
    public string EventType { get; init; } // "Marriage", "Birth", "Divorce"
    public DateTime EventDate { get; init; }
    public DateTime DeadlineDate { get; init; }
    public string Status { get; init; } // "Pending", "Processed", "Missed"
    public IEnumerable<Guid> AffectedPlanIds { get; init; } = new List<Guid>();
    public string DocumentationRequirements { get; init; }
}

public record BenefitsDocumentStatusDto
{
    public Guid DocumentId { get; init; }
    public string DocumentName { get; init; }
    public string DocumentType { get; init; } // "Enrollment Form", "Proof of Relationship"
    public DateTime? SubmittedDate { get; init; }
    public string Status { get; init; } // "Received", "Pending Review", "Rejected"
    public string RejectionReason { get; init; }
    public string UploadUrl { get; init; }
}

// Extension methods for richer functionality
public static class BenefitsSummaryExtensions
{
    public static decimal CalculateTotalSavings(this BenefitsSummaryDto summary, BenefitsSummaryDto previousYear)
    {
        return previousYear.TotalAnnualEmployeeCost - summary.TotalAnnualEmployeeCost;
    }
    
    public static IEnumerable<ActiveBenefitPlanDto> GetExpiringPlans(this BenefitsSummaryDto summary)
    {
        return summary.ActivePlans
            .Where(p => p.TerminationDate.HasValue && 
                       p.TerminationDate.Value > summary.AsOfDate &&
                       p.TerminationDate.Value < summary.AsOfDate.AddMonths(3))
            .OrderBy(p => p.TerminationDate);
    }
    
    public static bool HasPendingDocuments(this BenefitsSummaryDto summary)
    {
        return summary.DocumentStatuses.Any(d => 
            d.Status == "Pending Review" || 
            d.Status == "Rejected");
    }
}