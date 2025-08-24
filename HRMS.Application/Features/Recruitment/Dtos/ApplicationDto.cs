namespace HRMS.Application.Features.Recruitment.Dtos;

public class ApplicationDto
{
    public Guid Id { get; set; }
    public string CandidateName { get; set; }
    public string CandidateEmail { get; set; }
    public string Status { get; set; }
    public DateTime AppliedDate { get; set; }
}