using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.TimeTrackingAggregate;

public class TimeEntry : Entity<Guid>, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly ClockIn { get; private set; }
    public TimeOnly? ClockOut { get; private set; }
    public int BreakTimeMinutes { get; private set; }
    public string? Description { get; private set; }
    public string? Project { get; private set; }
    public decimal TotalHours { get; private set; }
    public TimeEntryStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<TimeTrackingLocation> _locations = new();
    public IReadOnlyCollection<TimeTrackingLocation> Locations => _locations.AsReadOnly();

    private TimeEntry()
    {
    } // EF Core

    public TimeEntry(Guid id, Guid employeeId, DateOnly date, TimeOnly clockIn, decimal totalHours)
    {
        Id = id;
        EmployeeId = employeeId;
        Date = date;
        ClockIn = clockIn;
        TotalHours = totalHours;
        BreakTimeMinutes = 0;
        Status = TimeEntryStatus.Active;
        CreatedAt = DateTime.Now;
        //UpdatedAt = DateTime.UtcNow;
    }

    public void SetClockOut(TimeOnly clockOut)
    {
        ClockOut = clockOut;
        UpdateTimestamp();
    }

    
    public void SetBreakTime(int minutes)
    {
        BreakTimeMinutes = minutes;
        UpdateTimestamp();
    }

    public void SetDescription(string? description)
    {
        Description = description;
        UpdateTimestamp();
    }

    public void SetProject(string? project)
    {
        Project = project;
        UpdateTimestamp();
    }

    public void MarkCompleted()
    {
        Status = TimeEntryStatus.Completed;
        UpdateTimestamp();
    }

    public void AddLocation(TimeTrackingLocation location)
    {
        _locations.Add(location);
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTime.Now;
    }
}