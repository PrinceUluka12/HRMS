using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.RecruitmentAggregates;

public class Attachment : Entity<Guid>
{
    public string FileName { get; set; }
    
    public string FileType { get; set; }
    public string FileUrl { get; set; }

    private Attachment()
    {
    }

    public Attachment(string fileName, string fileType, string fileUrl)
    {
        Id = Guid.NewGuid();
        FileUrl = fileUrl;
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        FileType = fileType ?? throw new ArgumentNullException(nameof(fileType));
    }
}