using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.PositionAggregate;

public class Position : Entity<Guid>, IAggregateRoot
{
    public string Title { get; private set; }
    public string Code { get; private set; }
    public decimal BaseSalary { get; private set; }
    public string Description { get; private set; }
    public Guid DepartmentId { get; private set; }
    private Position() { }

    public Position(string title, string code, decimal baseSalary, string description, Guid departmentId)
    {
        Id = Guid.NewGuid();
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Code = code ?? throw new ArgumentNullException(nameof(code));
        BaseSalary = baseSalary;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        DepartmentId = departmentId;
    }

    public void UpdateDetails(string title, string code, decimal baseSalary, string description)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Code = code ?? throw new ArgumentNullException(nameof(code));
        BaseSalary = baseSalary;
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public void UpdateSalary(decimal newBaseSalary)
    {
        BaseSalary = newBaseSalary;
    }
}