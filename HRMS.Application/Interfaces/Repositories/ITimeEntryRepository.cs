

using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.TimeTrackingAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface ITimeEntryRepository : IGenericRepository<TimeEntry>
{
    Task<IEnumerable<TimeEntry>> GetEntryByEmployeeId(Guid EmployeeId,CancellationToken cancellationToken = default);
}