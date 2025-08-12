using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Enums;

namespace HRMS.Application.Interfaces;

public interface IEquipmentRepository : IGenericRepository<Equipment>
{
    Task<List<Equipment>> GetByTypeAsync(EquipmentType type, CancellationToken cancellationToken = default);
    
    
    
}