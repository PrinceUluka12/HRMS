using HRMS.Application.Common.Exceptions;
using HRMS.Application.Features.Benefits.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Interfaces.Services;

public class BenefitsService(
    IEmployeeRepository employeeRepository,
    ILogger<BenefitsService> logger)
    : IBenefitsService
{
    private const decimal DefaultBenefitsRate = 0.12m; // 12% of base salary

    public async Task<decimal> CalculateBenefitsDeductionsAsync(Guid employeeId)
    {
        var employee = await employeeRepository.GetByIdWithIncludesAsync(employeeId, e => e.Position);
        
        if (employee == null)
        {
            logger.LogWarning("Employee {EmployeeId} not found for benefits calculation", employeeId);
            throw new NotFoundException(nameof(Employee), employeeId);
        }

        if (employee.Position == null)
        {
            logger.LogWarning("Employee {EmployeeId} does not have an associated position", employeeId);
            throw new InvalidOperationException("Employee does not have an assigned position.");
        }

        var deduction = employee.Position.BaseSalary * DefaultBenefitsRate;

        logger.LogInformation("Calculated benefit deduction of {Deduction:C} for employee {EmployeeId}", deduction, employeeId);

        return deduction;
    }

    public Task<BenefitsEnrollmentResult> EnrollEmployeeAsync(Guid employeeId, IEnumerable<Guid> benefitPlanIds)
    {
        logger.LogInformation("EnrollEmployeeAsync not yet implemented for employee {EmployeeId}", employeeId);
        throw new NotImplementedException("Enrollment logic will handle adding benefit plan relations and validations.");
    }

    public Task<BenefitsSummaryDto> GetBenefitsSummaryAsync(Guid employeeId)
    {
        logger.LogInformation("GetBenefitsSummaryAsync not yet implemented for employee {EmployeeId}", employeeId);
        throw new NotImplementedException("Benefits summary will aggregate plan data, usage, deductions, etc.");
    }
}
