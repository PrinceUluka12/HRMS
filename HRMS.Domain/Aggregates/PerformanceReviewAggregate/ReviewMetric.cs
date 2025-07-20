using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.PerformanceReviewAggregate;

public class ReviewMetric : Entity<Guid>
{
    public Guid PerformanceReviewId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Weight { get; private set; }
    public decimal Rating { get; private set; }
    public string Comments { get; private set; }

    private ReviewMetric() { }

    public ReviewMetric(
        Guid performanceReviewId,
        string name,
        string description,
        decimal weight)
    {
        PerformanceReviewId = performanceReviewId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Weight = weight;
        Rating = 0;
        Comments = string.Empty;
    }

    public void UpdateRating(decimal rating, string comments)
    {
        if (rating < 0 || rating > 5)
            throw new DomainException("Rating must be between 0 and 5");

        Rating = rating;
        Comments = comments ?? throw new ArgumentNullException(nameof(comments));
    }
}