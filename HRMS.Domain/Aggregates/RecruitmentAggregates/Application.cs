
using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.RecruitmentAggregates;

public class Application : Entity<Guid>
{
    public Guid JobVacancyId { get; private set; } // Reference to the JobVacancy aggregate
    public Candidate CandidateInfo { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public DateTime AppliedDate { get; private set; }
    private readonly List<string> _notes = new();
    private readonly List<Attachment> _attachments = new();
    public ICollection<ApplicationNote> Notes { get; set; }
    public IReadOnlyCollection<Attachment> Attachments => _attachments.AsReadOnly();


    private Application()
    {
    }

    public Application(Guid jobVacancyId, Candidate candidate)
    {
        Id = Guid.NewGuid();
        JobVacancyId = jobVacancyId;
        CandidateInfo = candidate ?? throw new ArgumentNullException(nameof(candidate));
        Status = ApplicationStatus.Applied;
        AppliedDate = DateTime.UtcNow;
    }

    public void ChangeStatus(ApplicationStatus newStatus)
    {
        // Add business rules if needed, e.g., disallow some transitions
        Status = newStatus;
    }

    public void AddNote(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
            throw new ArgumentException("Note cannot be empty.", nameof(note));
        ApplicationNote newNote = new ApplicationNote()
        {
            Value = note
        };
        Notes.Add(newNote);
    }
    
    public void AddAttachment(Attachment attachment)
    {
        _attachments.Add(attachment ?? throw new ArgumentNullException(nameof(attachment)));
    }
}