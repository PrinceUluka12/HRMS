using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.TimeTrackingAggregate;

public class TimeTrackingLocation : Entity<Guid>
{
    public Guid TimeEntryId { get; private set; }
    public ClockActionType ActionType { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public DateTime Timestamp { get; private set; }

    private TimeTrackingLocation() { } // EF Core

    public TimeTrackingLocation(Guid id, Guid timeEntryId, ClockActionType actionType, DateTime timestamp,
        decimal? latitude = null, decimal? longitude = null)
    {
        Id = id;
        TimeEntryId = timeEntryId;
        ActionType = actionType;
        Timestamp = timestamp;
        Latitude = latitude;
        Longitude = longitude;
    }
}