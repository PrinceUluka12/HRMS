namespace HRMS.Application.Features.Recruitment.Dtos;

public class JobVacancyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string EmploymentType { get; set; }
    public string Status { get; set; }
    public DateTime? PostedOn { get; set; }
    public DateTime? ClosingOn { get; set; }
    public List<ApplicationDto> Applications { get; set; }
}