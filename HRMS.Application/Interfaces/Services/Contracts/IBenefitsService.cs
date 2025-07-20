using HRMS.Application.Features.Benefits.Dtos;

namespace HRMS.Application.Interfaces.Services.Contracts;

public interface IBenefitsService
{
    Task<decimal> CalculateBenefitsDeductionsAsync(Guid employeeId);
    Task<BenefitsEnrollmentResult> EnrollEmployeeAsync(Guid employeeId, IEnumerable<Guid> benefitPlanIds);
    Task<BenefitsSummaryDto> GetBenefitsSummaryAsync(Guid employeeId);
}