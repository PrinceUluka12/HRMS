using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class Onboarding : Entity<Guid>, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public OnboardingStatus Status { get; private set; }
    public int OverallProgress { get; private set; } // 0–100
    public int DaysRemaining { get; private set; }

    private readonly List<OnboardingStage> _stages = new();
    public IReadOnlyCollection<OnboardingStage> Stages => _stages.AsReadOnly();

    private readonly List<OnboardingDocument> _documents = new();
    public IReadOnlyCollection<OnboardingDocument> Documents => _documents.AsReadOnly();

    public DateTime LastActivity { get; private set; }
    public DateTime CreatedDate { get; private set; }

    private Onboarding() { } // EF Core

    public Onboarding(Guid employeeId, DateTime startDate)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        StartDate = startDate;
        CreatedDate = DateTime.UtcNow;
        LastActivity = CreatedDate;
        Status = OnboardingStatus.Pending;
        OverallProgress = 0;
        DaysRemaining = CalculateDaysRemaining();
    }

    public void AddStage(OnboardingStage stage)
    {
        _stages.Add(stage);
        RecalculateProgress();
    }

    public void AddDocument(OnboardingDocument document)
    {
        _documents.Add(document);
        LastActivity = DateTime.UtcNow;
    }

    public void UpdateStatus(OnboardingStatus status)
    {
        Status = status;
        LastActivity = DateTime.UtcNow;
    }

    public void RecalculateProgress()
    {
        if (!_stages.Any())
        {
            OverallProgress = 0;
            Status = OnboardingStatus.Pending;
            return;
        }

        var totalProgress = _stages.Sum(s => s.Progress);
        OverallProgress = totalProgress / _stages.Count;

        Status = OverallProgress switch
        {
            100 => OnboardingStatus.Completed,
            > 0 => OnboardingStatus.InProgress,
            _ => OnboardingStatus.Pending
        };

        DaysRemaining = CalculateDaysRemaining();
        LastActivity = DateTime.UtcNow;
    }

    private int CalculateDaysRemaining()
    {
        var latestDueDate = _stages
            .SelectMany(s => s.Tasks)
            .Max(t => (DateTime?)t.DueDate);

        if (latestDueDate == null)
            return 0;

        var remaining = (latestDueDate.Value.Date - DateTime.UtcNow.Date).Days;
        return remaining < 0 ? 0 : remaining;
    }
}
