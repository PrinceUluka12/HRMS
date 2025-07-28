namespace HRMS.Application.Features.Employees.Dtos;

public record EmployeeListDto
(
    Guid id,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email ,
    string WorkPhone ,
    string DepartmentName,
    string PositionTitle,
    string Status,
    DateTime HireDate
);