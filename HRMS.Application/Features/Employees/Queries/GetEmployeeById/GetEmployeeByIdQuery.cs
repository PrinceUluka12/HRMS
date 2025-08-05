using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Features.Performance.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<BaseResult<EmployeeDetailDto>>;

public class GetEmployeeByIdQueryHandler(
    IEmployeeRepository employeeRepository,
    ITranslator translator,
    ILogger<GetEmployeeByIdQueryHandler> logger)
    : IRequestHandler<GetEmployeeByIdQuery, BaseResult<EmployeeDetailDto>>
{
    public async Task<BaseResult<EmployeeDetailDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await employeeRepository.GetByIdWithIncludesAsync(
                request.Id,
                e => e.Department,
                e => e.Position,
                e => e.Dependents,
                e => e.EmergencyContacts,
                e => e.Certifications,
                e => e.EducationHistory,
                e => e.LeaveRequests,
                e => e.PerformanceReviews);

            if (employee is null)
            {
                return BaseResult<EmployeeDetailDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString(TranslatorMessages.EmployeeMessages.Employee_NotFound_with_id(request.Id)),
                    nameof(request.Id)
                ));
            }

            var managerId = employee.Department?.ManagerId;
            var managerName = managerId.HasValue ? await TryGetManagerName(managerId.Value) : "";

            var dto = new EmployeeDetailDto
            {
                Id = employee.Id,
                EmployeeNumber = employee.EmployeeNumber,
                GovernmentId = employee.GovernmentId,
                TaxIdentificationNumber = employee.TaxIdentificationNumber,
                FirstName = employee.Name.FirstName,
                LastName = employee.Name.LastName,
                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender.ToString(),
                MaritalStatus = employee.MaritalStatus.ToString(),
                Email = employee.Email.Value,
                WorkPhone = employee.WorkPhone,
                PersonalPhone = employee.PersonalPhone,
                PrimaryAddress = employee.PrimaryAddress is not null
                    ? new AddressDto(
                        employee.PrimaryAddress.Street,
                        employee.PrimaryAddress.City,
                        employee.PrimaryAddress.State,
                        employee.PrimaryAddress.PostalCode,
                        employee.PrimaryAddress.Country)
                    : null,
                HireDate = employee.HireDate,
                TerminationDate = employee.TerminationDate,
                Status = employee.Status.ToString(),
                EmploymentType = employee.EmploymentType.ToString(),
                IsFullTime = employee.IsFullTime,
                FullTimeEquivalent = employee.FullTimeEquivalent,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name ?? "",
                PositionId = employee.PositionId,
                PositionTitle = employee.Position?.Title ?? "",
                ManagerId = managerId,
                ManagerName = managerName,
                JobTitle = employee.Position?.Title ?? "",
                BaseSalary = employee.BaseSalary,
                PayFrequency = employee.PayFrequency.ToString(),
                EmergencyContacts = employee.EmergencyContacts
                    .Select(c => new EmergencyContactDto(
                        c.Name,
                        c.Relationship,
                        c.PhoneNumber,
                        c.Email))
                    .ToList(),

                Dependents = employee.Dependents
                    .Select(d => new DependentDto(
                        d.Name,
                        d.DateOfBirth,
                        d.Relationship.ToString(),
                        d.IsForTaxBenefits))
                    .ToList(),

                Certifications = employee.Certifications
                    .Select(cert => new CertificationDto(
                        cert.Id,
                        cert.Name,
                        cert.IssuingOrganization,
                        cert.IssueDate,
                        cert.ExpirationDate))
                    .ToList(),

                EducationHistory = employee.EducationHistory
                    .Select(e => new EducationDto(
                        e.Institution,
                        e.Degree,
                        e.FieldOfStudy,
                        e.StartDate,
                        e.EndDate,
                        e.IsCompleted))
                    .ToList(),

                LeaveRequests = employee.LeaveRequests
                    .Select(l => new LeaveRequestDto
                    {
                        Id = l.Id,
                        EmployeeId = l.EmployeeId,
                        EmployeeName = $"{employee.Name.FirstName} {employee.Name.LastName}",
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                        Type = l.Type,
                        Reason = l.Reason,
                        Status = l.Status,
                        RequestDate = l.RequestDate,
                        ApprovedBy = l.ApprovedBy,
                        ApprovedDate = l.ApprovedDate,
                        RejectionReason = l.RejectionReason
                    })
                    .ToList(),

                PerformanceReviews = employee.PerformanceReviews
                    .Select(r => new PerformanceReviewDto(
                        r.Id,
                        r.EmployeeId,
                        $"{employee.Name.FirstName} {employee.Name.LastName}",
                        r.ReviewerId,
                        r.EmployeeId.ToString(), // Can be replaced with reviewer name
                        r.ReviewDate,
                        r.NextReviewDate,
                        r.OverallRating,
                        r.OverallComments,
                        r.Goals.Select(g => new PerformanceGoalDto(
                            g.Id,
                            g.Description,
                            g.TargetDate,
                            g.Status,
                            g.Comments)).ToList()))
                    .ToList()
            };

            return BaseResult<EmployeeDetailDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving employee with ID: {EmployeeId}", request.Id);

            return BaseResult<EmployeeDetailDto>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }

    private async Task<string> TryGetManagerName(Guid managerId)
    {
        try
        {
            var manager = await employeeRepository.GetByIdAsync(managerId);
            return manager is null ? "" : $"{manager.Name.FirstName} {manager.Name.LastName}";
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to resolve manager name for ID: {ManagerId}", managerId);
            return "";
        }
    }
}
