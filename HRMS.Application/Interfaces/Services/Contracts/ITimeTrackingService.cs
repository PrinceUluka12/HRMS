//using HRMS.Domain.Aggregates.TimeTrackingAggregate;

using HRMS.Application.Features.TimeTracking.Dtos;
using HRMS.Domain.Aggregates.TimeTrackingAggregate;
using HRMS.Domain.Enums;

namespace HRMS.Application.Interfaces.Services.Contracts;

public interface ITimeTrackingService
{
    Task<Guid> ClockInAsync(Guid employeeId, DateOnly date, TimeOnly time, decimal latitude, decimal longitude);
    Task ClockOutAsync(Guid timeEntryId, TimeOnly time, decimal latitude, decimal longitude);
    Task AddLocationAsync(Guid timeEntryId, ClockActionType actionType, decimal latitude, decimal longitude, DateTime timestamp);
    Task CompleteEntryAsync(Guid timeEntryId);
    Task<IEnumerable<TimeEntry>> GetByEmployeeIdAsync(Guid id);
    
    Task<Guid> ManualEntryAsync( ManualEntryDto request);
}