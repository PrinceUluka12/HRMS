using HRMS.Domain.Enums;

namespace HRMS.Domain.Aggregates.EmployeeAggregate;

public record PersonName(string FirstName, string LastName);



public record Address(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country);

public record BankDetails(
    string BankName,
    string AccountNumber,
    string RoutingNumber);
    
// Phone Number Value Object


// Additional Value Objects


public record Dependent(
    string Name,
    DateTime DateOfBirth,
    DependentRelationship Relationship,
    bool IsForTaxBenefits);

public record Education(
    string Institution,
    string Degree,
    string FieldOfStudy,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCompleted);

