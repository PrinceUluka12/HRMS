using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.OnboardingAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IEquipmentAssignmentRepository : IGenericRepository<EquipmentAssignment>
{
    Task<List<EquipmentAssignment>> GetByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<List<EquipmentAssignment>> GetActiveEquipmentAsync(CancellationToken cancellationToken = default);
}