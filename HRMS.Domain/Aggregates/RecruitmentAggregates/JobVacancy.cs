using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.RecruitmentAggregates;

public class JobVacancy : Entity<Guid>, IAggregateRoot
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public DateTime? PostedOn { get; set; }
    public DateTime ClosingOn { get; set; }
    public JobVacancyStatus Status { get; set; }
    
    private readonly List<Application> _applications = new();
    public IReadOnlyCollection<Application> Applications => _applications.AsReadOnly();


    private JobVacancy()
    {
    }

    public JobVacancy(string title, string description, string location,
        EmploymentType employmentType, DateTime closingOn)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Location = location;
        EmploymentType = employmentType;
        PostedOn = DateTime.Now;
        ClosingOn = closingOn;
        Status = JobVacancyStatus.Open;
    }
    
    public void Publish()
    {
        if (Status == JobVacancyStatus.Published)
            throw new DomainException("Job is already published.");

        Status = JobVacancyStatus.Published;
        PostedOn = DateTime.Now;
        // Raise domain event JobVacancyPublished if using event system
    }

    public void AddApplication(Application application)
    {
        if (Status != JobVacancyStatus.Published)
            throw new InvalidOperationException("Cannot apply to unpublished job.");

        _applications.Add(application);
    }
    
    public void Close()
    {
        Status = JobVacancyStatus.Closed;
    }
}