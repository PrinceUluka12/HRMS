using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class BuddyCheckIn : Entity<Guid>
{
    public Guid BuddyPairId { get; set; }
    public DateTime CheckInDate { get; private set; }
    public string? MentorNotes { get; private set; }
    public string? MenteeNotes { get; private set; }
    public int? Rating { get; private set; }
    public List<string>? Topics { get; private set; }
    public DateTime? NextCheckInDate { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private BuddyCheckIn()
    {
    }

    public BuddyCheckIn(Guid buddyPairId, DateTime checkInDate, Guid createdBy, List<string>? topics = null,
        string? mentorNotes = null, string? menteeNotes = null, int? rating = null, DateTime? nextCheckInDate = null)
    {
        Id = Guid.NewGuid();
        BuddyPairId = buddyPairId;
        CheckInDate = checkInDate;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        Topics = topics;
        MentorNotes = mentorNotes;
        MenteeNotes = menteeNotes;
        Rating = rating;
        NextCheckInDate = nextCheckInDate;
    }
}