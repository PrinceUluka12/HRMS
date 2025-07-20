namespace HRMS.Application.Features.Employees.Dtos;

public record AddressDto(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country);