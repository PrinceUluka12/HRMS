using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class BuddyPair: Entity<Guid>, IAggregateRoot
{
    public Guid MentorId { get; private set; }
    public Guid MenteeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public BuddyPairStatus Status { get; private set; }
    public Guid AssignedBy { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<BuddyCheckIn> _checkIns = new();
    public IReadOnlyCollection<BuddyCheckIn> CheckIns => _checkIns.AsReadOnly();

    private BuddyPair() { }
    
    public BuddyPair(Guid mentorId, Guid menteeId, DateTime startDate, Guid assignedBy, string? notes = null)
    {
        Id = Guid.NewGuid();
        MentorId = mentorId;
        MenteeId = menteeId;
        StartDate = startDate;
        Status = BuddyPairStatus.Active;
        AssignedBy = assignedBy;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void AddCheckIn(BuddyCheckIn checkIn)
    {
        _checkIns.Add(checkIn);
        UpdatedAt = DateTime.UtcNow;
    }

    
    public void Close(DateTime endDate)
    {
        EndDate = endDate;
        Status = BuddyPairStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public static BuddyPair Create(Guid mentorId, Guid menteeId, DateTime startDate, Guid assignedBy, string? notes = null)
    {
        if (mentorId == Guid.Empty)
            throw new ArgumentException("MentorId must be a valid GUID.", nameof(mentorId));

        if (menteeId == Guid.Empty)
            throw new ArgumentException("MenteeId must be a valid GUID.", nameof(menteeId));

        if (startDate.Date < DateTime.UtcNow.Date)
            throw new ArgumentException("Start date cannot be in the past.", nameof(startDate));

        if (assignedBy == Guid.Empty)
            throw new ArgumentException("AssignedBy must be a valid GUID.", nameof(assignedBy));

        return new BuddyPair(mentorId, menteeId, startDate, assignedBy, notes);
    }

}