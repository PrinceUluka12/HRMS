using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.PerformanceReviewAggregate;

public class PerformanceGoal : Entity<Guid>, IAggregateRoot
{
    public Guid PerformanceReviewId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime TargetDate { get; private set; }
    public PerformanceGoalStatus Status { get; private set; }
    public decimal Progress { get; private set; }
    public string Comments { get; private set; }

    private PerformanceGoal() { }

    public PerformanceGoal(
        Guid performanceReviewId,
        string title,
        string description,
        DateTime targetDate)
    {
        PerformanceReviewId = performanceReviewId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        TargetDate = targetDate;
        Status = PerformanceGoalStatus.NotStarted;
        Progress = 0;
        Comments = string.Empty;
    }

    public void UpdateProgress(decimal progress, string comments)
    {
        if (progress < 0 || progress > 100)
            throw new DomainException("Progress must be between 0 and 100");

        Progress = progress;
        Comments = comments ?? throw new ArgumentNullException(nameof(comments));

        Status = progress switch
        {
            0 => PerformanceGoalStatus.NotStarted,
            100 => PerformanceGoalStatus.Achieved,
            _ => PerformanceGoalStatus.InProgress
        };
    }

    public void MarkAsComplete(string comments)
    {
        Progress = 100;
        Status = PerformanceGoalStatus.Achieved;
        Comments = comments ?? throw new ArgumentNullException(nameof(comments));
    }
}