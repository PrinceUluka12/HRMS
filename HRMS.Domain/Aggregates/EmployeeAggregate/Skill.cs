using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.EmployeeAggregate;

public class Skill : Entity<Guid>
{
    public string Name { get; private set; }
    public SkillLevel Level { get; private set; }
    public DateTime AcquiredDate { get; private set; }

    private Skill() { }

    public Skill(string name, SkillLevel level, DateTime acquiredDate)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Level = level;
        AcquiredDate = acquiredDate;
    }
}

public enum SkillLevel
{
    Beginner,
    Intermediate,
    Advanced,
    Expert
}