using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.PositionAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IPositionRepository : IGenericRepository<Position>
{
    Task<Position?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
}