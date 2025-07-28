using HRMS.Domain.Enums;

namespace HRMS.Application.Features.TimeTracking.Dtos;

public class ClockInRequest
{
    public Guid EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}

public class ClockOutRequest
{
    public Guid TimeEntryId { get; set; }
    public TimeOnly Time { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}

public class LocationRequest
{
    public Guid TimeEntryId { get; set; }
    public ClockActionType ActionType { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime Timestamp { get; set; }
}
