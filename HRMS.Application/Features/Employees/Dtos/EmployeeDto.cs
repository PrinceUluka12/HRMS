using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Employees.Dtos;

public record EmployeeDto(
    Guid Id,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    DateTime HireDate,
    Guid DepartmentId,
    string DepartmentName,
    Guid PositionId,
    string PositionTitle,
    EmploymentStatus Status);