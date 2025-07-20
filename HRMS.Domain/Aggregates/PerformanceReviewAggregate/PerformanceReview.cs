using HRMS.Domain.Aggregates.PerformanceReviewAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.PerformanceAggregate;

public class PerformanceReview : Entity<Guid>, IAggregateRoot
{
    // Core review information
    public Guid EmployeeId { get; private set; }
    public Guid ReviewerId { get; private set; }
    public DateTime ReviewDate { get; private set; }
    public DateTime? NextReviewDate { get; private set; }
    public PerformanceReviewStatus Status { get; private set; }
    public string OverallComments { get; private set; }
    public decimal OverallRating { get; private set; }

    // Review metrics
    private readonly List<ReviewMetric> _metrics = new();
    public IReadOnlyCollection<ReviewMetric> Metrics => _metrics.AsReadOnly();

    // Goals and development plans
    private readonly List<PerformanceGoal> _goals = new();
    public IReadOnlyCollection<PerformanceGoal> Goals => _goals.AsReadOnly();

    // Feedback from others
    private readonly List<Feedback> _feedback = new();
    public IReadOnlyCollection<Feedback> Feedback => _feedback.AsReadOnly();

    // Private constructor for EF Core
    private PerformanceReview() { }

    public PerformanceReview(
        Guid employeeId,
        Guid reviewerId,
        DateTime reviewDate,
        string overallComments)
    {
        EmployeeId = employeeId;
        ReviewerId = reviewerId;
        ReviewDate = reviewDate;
        OverallComments = overallComments ?? throw new ArgumentNullException(nameof(overallComments));
        Status = PerformanceReviewStatus.Draft;
        CalculateOverallRating();
    }

    // Domain methods
    public void AddMetric(ReviewMetric metric)
    {
        if (metric == null) throw new ArgumentNullException(nameof(metric));
        _metrics.Add(metric);
        CalculateOverallRating();
    }

    public void AddGoal(PerformanceGoal goal)
    {
        if (goal == null) throw new ArgumentNullException(nameof(goal));
        _goals.Add(goal);
    }

    public void AddFeedback(Feedback feedback)
    {
        if (feedback == null) throw new ArgumentNullException(nameof(feedback));
        _feedback.Add(feedback);
    }

    public void SubmitForApproval()
    {
        if (Status != PerformanceReviewStatus.Draft)
            throw new DomainException("Only draft reviews can be submitted");

        if (!_metrics.Any())
            throw new DomainException("Cannot submit review without metrics");

        Status = PerformanceReviewStatus.PendingApproval;
    }

    public void Approve(DateTime nextReviewDate)
    {
        if (Status != PerformanceReviewStatus.PendingApproval)
            throw new DomainException("Only pending reviews can be approved");

        Status = PerformanceReviewStatus.Approved;
        NextReviewDate = nextReviewDate;
    }

    public void Reject(string comments)
    {
        if (Status != PerformanceReviewStatus.PendingApproval)
            throw new DomainException("Only pending reviews can be rejected");

        Status = PerformanceReviewStatus.Rejected;
        OverallComments += $"\nRejection Comments: {comments}";
    }

    public void Cancel()
    {
        if (Status == PerformanceReviewStatus.Approved)
            throw new DomainException("Approved reviews cannot be canceled");

        Status = PerformanceReviewStatus.Canceled;
    }

    private void CalculateOverallRating()
    {
        if (_metrics.Any())
        {
            OverallRating = _metrics.Average(m => m.Rating);
        }
    }
}