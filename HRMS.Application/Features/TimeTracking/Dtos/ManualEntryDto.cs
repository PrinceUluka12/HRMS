namespace HRMS.Application.Features.TimeTracking.Dtos;

public class ManualEntryDto
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public string Project { get; set; }
    public string Description { get; set; }
    public Guid EmployeeId { get; set; }

    public decimal TotalHours { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}