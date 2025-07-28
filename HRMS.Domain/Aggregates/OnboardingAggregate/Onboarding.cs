using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class Onboarding: Entity<Guid>, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public OnboardingStage Stage { get; private set; }
    public List<OnboardingTask> Tasks { get; private set; } = new();

    private Onboarding() { }
    
    
    public Onboarding(Guid employeeId, DateTime startDate, List<OnboardingTask> defaultTasks)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        StartDate = startDate;
        Stage = OnboardingStage.PreOnboarding;
        Tasks = defaultTasks;
    }

    public void AdvanceStage()
    {
        if (Stage != OnboardingStage.Completed)
            Stage++;
    }
    
    /*public void MarkTaskComplete(string title)
    {
        var taskIndex = Tasks.FindIndex(t => t.Title == title);
        if (taskIndex >= 0)
        {
            var task = Tasks[taskIndex];
            Tasks[taskIndex] = task with { IsCompleted = true };
        }
    }*/
}