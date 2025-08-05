using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class OnboardingStage : Entity<Guid>
{
    public string StageName { get; private set; }
    public OnboardingStageStatus Status { get; private set; }
    public int Progress { get; private set; } // 0–100
    public DateTime DueDate { get; private set; }

    private readonly List<OnboardingTask> _tasks = new();
    public IReadOnlyCollection<OnboardingTask> Tasks => _tasks.AsReadOnly();

    private OnboardingStage() { } // EF Core

    public OnboardingStage(string stageName, DateTime dueDate)
    {
        Id = Guid.NewGuid();
        StageName = stageName;
        DueDate = dueDate;
        Status = OnboardingStageStatus.Pending;
        Progress = 0;
    }

    public void AddTask(OnboardingTask task)
    {
        _tasks.Add(task);
        RecalculateProgress();
    }

    public void UpdateStatus(OnboardingStageStatus status)
    {
        Status = status;
    }

    public void RecalculateProgress()
    {
        if (!_tasks.Any())
        {
            Progress = 0;
            Status = OnboardingStageStatus.Pending;
            return;
        }

        var completed = _tasks.Count(t => t.Status == OnboardingTaskStatus.Completed);
        Progress = (int)((double)completed / _tasks.Count * 100);

        if (Progress == 100)
            Status = OnboardingStageStatus.Completed;
        else if (Progress > 0)
            Status = OnboardingStageStatus.InProgress;
    }
}