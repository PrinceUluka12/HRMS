namespace HRMS.Application.Features.BuddyPair.Dtos;

public class BuddyCheckInDto
{
    public Guid Id { get; set; }
    public Guid BuddyPairId { get; set; }
    public DateTime CheckInDate { get; set; }
    public string MentorNotes { get; set; } = null!;
    public string MenteeNotes { get; set; } = null!;
    public int Rating { get; set; } // 1-5 scale
    public List<string> Topics { get; set; } = new();
    public DateTime? NextCheckInDate { get; set; }
    public Guid CreatedBy { get; set; }
}