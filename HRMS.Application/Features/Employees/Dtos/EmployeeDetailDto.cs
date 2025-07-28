using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Features.Performance.Dtos;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Employees.Dtos;

public record EmployeeDetailDto
{
    // Identification
    public Guid Id { get; init; }
    public string EmployeeNumber { get; init; }
    public string GovernmentId { get; init; }
    public string TaxIdentificationNumber { get; init; }

    // Personal Information
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string Gender { get; init; }
    public string MaritalStatus { get; init; }

    // Contact Information
    public string Email { get; init; }
    public string WorkPhone { get; init; }
    public string PersonalPhone { get; init; }
    public AddressDto PrimaryAddress { get; init; }

    // Employment Details
    public DateTime HireDate { get; init; }
    public DateTime? TerminationDate { get; init; }
    public string? TerminationReason { get; init; }
    public string Status { get; init; }
    public string EmploymentType { get; init; }
    public bool IsFullTime { get; init; }
    public decimal FullTimeEquivalent { get; init; }

    // Organizational Structure
    public Guid DepartmentId { get; init; }
    public string DepartmentName { get; init; }
    public Guid PositionId { get; init; }
    public string PositionTitle { get; init; }
    public Guid? ManagerId { get; init; }
    public string? ManagerName { get; init; }
    public string JobTitle { get; init; }
    

    // Compensation
    public decimal BaseSalary { get; init; }
    public string PayFrequency { get; init; }
    public BankDetailsDto BankDetails { get; init; }

    // Collections
    public IEnumerable<EmergencyContactDto>? EmergencyContacts { get; init; }
    public IEnumerable<DependentDto> Dependents { get; init; }
    public IEnumerable<SkillDto> Skills { get; init; }
    public IEnumerable<CertificationDto> Certifications { get; init; }
    public IEnumerable<EducationDto> EducationHistory { get; init; }
    public IEnumerable<LeaveRequestDto> LeaveRequests { get; init; }
    public IEnumerable<PerformanceReviewDto> PerformanceReviews { get; init; }
}

// Supporting DTOs for collections
public record EmergencyContactDto(
    string Name,
    string Relationship,
    string PhoneNumber,
    string Email);

public record DependentDto(
    string Name,
    DateTime DateOfBirth,
    string Relationship,
    bool IsForTaxBenefits);

public record EducationDto(
    string Institution,
    string Degree,
    string FieldOfStudy,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCompleted);