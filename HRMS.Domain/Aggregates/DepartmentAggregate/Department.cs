using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.DepartmentAggregate;

public class Department : Entity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public Guid? ManagerId { get; private set; }
    public Employee Manager { get; private set; }
    public string Description { get; private set; }
    

    private Department() { }

    public Department(string name, string code, string description, Guid? managerId = null)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ManagerId = managerId;
    }

    public void UpdateDetails(string name, string code, string description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public void AssignManager(Guid managerId)
    {
        ManagerId = managerId;
    }

    
}