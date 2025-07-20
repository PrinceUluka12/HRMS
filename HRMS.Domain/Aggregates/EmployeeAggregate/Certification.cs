using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.EmployeeAggregate;

public class Certification : Entity<Guid>
{
    public string Name { get; private set; }
    public string IssuingOrganization { get; private set; }
    public DateTime IssueDate { get; private set; }
    public DateTime? ExpirationDate { get; private set; }

    private Certification() { }

    public Certification(
        string name, 
        string issuingOrganization, 
        DateTime issueDate, 
        DateTime? expirationDate)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IssuingOrganization = issuingOrganization ?? throw new ArgumentNullException(nameof(issuingOrganization));
        IssueDate = issueDate;
        ExpirationDate = expirationDate;
    }
}