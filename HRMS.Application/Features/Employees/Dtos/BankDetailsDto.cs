namespace HRMS.Application.Features.Employees.Dtos;

public record BankDetailsDto(
    string BankName,
    string AccountNumber,
    string RoutingNumber);