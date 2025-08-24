using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.RecruitmentAggregates;

public class Candidate : Entity<Guid>, IAggregateRoot
{
    public Guid JobVacancyId { get; set; }
    public PersonName Name { get; set; } 
    public string Email { get; set; } 
    
    public string PhoneNumber { get; set; }
    public string ResumeUrl { get; set; }
    public DateTime AppliedOn { get; set; } 
    public  CandidateStatus Status{ get; set; } 
    
    private Candidate() { }

    public Candidate(Guid jobVacancyId, PersonName name, string email, string resumeUrl, string phoneNumber)
    {
        Id = Guid.NewGuid();
        JobVacancyId = jobVacancyId;
        Name = name;
        Email = email;
        ResumeUrl = resumeUrl;
        AppliedOn = DateTime.Now;
        Status = CandidateStatus.Created;
        PhoneNumber = phoneNumber;
    }

    
}