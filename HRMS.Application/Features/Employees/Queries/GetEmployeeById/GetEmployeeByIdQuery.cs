using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Features.Performance.Dtos;
using HRMS.Application.Interfaces.Repositories;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeDetailDto>;

public class GetEmployeeByIdQueryHandler(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IPositionRepository positionRepository) : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailDto>
{
    public async Task<EmployeeDetailDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await employeeRepository.GetByIdAsyncIncludeRelationship(
                request.Id,
                e => e.Department,
                f => f.Position,
                d => d.Dependents,
                e => e.EmergencyContacts,
                e => e.Certifications,
                e => e.EducationHistory,
                e => e.LeaveRequests,
                e => e.PerformanceReviews);
            EmployeeDetailDto data = new EmployeeDetailDto
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
                PrimaryAddress = new AddressDto(employee.PrimaryAddress.Street, employee.PrimaryAddress.City,
                    employee.PrimaryAddress.State, employee.PrimaryAddress.PostalCode, employee.PrimaryAddress.Country),
                HireDate = employee.HireDate,
                TerminationDate = employee.TerminationDate,
                Status = employee.Status.ToString(),
                EmploymentType = employee.EmploymentType.ToString(),
                IsFullTime = employee.IsFullTime,
                FullTimeEquivalent = employee.FullTimeEquivalent,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department.Name,
                PositionId = employee.PositionId,
                PositionTitle = employee.Position.Title,
                ManagerId = employee.Department.ManagerId,
                ManagerName = await GetEmployeeNameById(employee.Department.ManagerId),
                JobTitle = employee.Position.Title,
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
                    .Select(e => new CertificationDto(
                        e.Id,
                        e.Name,
                        e.IssuingOrganization,
                        e.IssueDate,
                        e.ExpirationDate
                    )).ToList(),

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
                    .Select(l => new LeaveRequestDto(
                        l.Id,
                        l.EmployeeId,
                        $"{employee.Name.FirstName} {employee.Name.LastName}", // Assuming employee name from current employee
                        l.StartDate,
                        l.EndDate,
                        l.Type,
                        l.Reason,
                        l.Status,
                        l.RequestDate,
                        l.ApprovedBy,
                        l.ApprovedDate,
                        l.RejectionReason
                    ))
                    .ToList(),

                PerformanceReviews = employee.PerformanceReviews
                    .Select(r => new PerformanceReviewDto(
                        r.Id,
                        r.EmployeeId,
                        $"{employee.Name.FirstName} {employee.Name.LastName}", // Assumes current employee's name
                        r.ReviewerId,
                        r.EmployeeId.ToString(), // If this exists on the entity; otherwise you'll need to resolve it
                        r.ReviewDate,
                        r.NextReviewDate,
                        r.OverallRating,
                        r.OverallComments,
                        r.Goals.Select(g => new PerformanceGoalDto(
                            g.Id,
                            g.Description,
                            g.TargetDate,
                            g.Status,
                            g.Comments
                        )).ToList()
                    ))
                    .ToList(),

            };
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<string> GetEmployeeNameById(Guid? id)
    {
        try
        {
            var data = await employeeRepository.GetByIdAsync(id.Value);
            return $"{data.Name.FirstName} {data.Name.LastName}";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}