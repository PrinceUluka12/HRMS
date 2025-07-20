using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.PerformanceReviewAggregate;

public class Feedback : Entity<Guid>, IAggregateRoot
{
    public Guid PerformanceReviewId { get; private set; }
    public Guid ProviderId { get; private set; }
    public DateTime ProvidedDate { get; private set; }
    public string Comments { get; private set; }
    public FeedbackType Type { get; private set; }

    private Feedback() { }

    public Feedback(
        Guid performanceReviewId,
        Guid providerId,
        string comments,
        FeedbackType type)
    {
        PerformanceReviewId = performanceReviewId;
        ProviderId = providerId;
        Comments = comments ?? throw new ArgumentNullException(nameof(comments));
        Type = type;
        ProvidedDate = DateTime.UtcNow;
    }

    public void UpdateComments(string comments)
    {
        Comments = comments ?? throw new ArgumentNullException(nameof(comments));
    }
}