using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class EquipmentRepository(ApplicationDbContext context)
    : GenericRepository<Equipment>(context), IEquipmentRepository
{
    public async Task<List<Equipment>> GetByTypeAsync(EquipmentType type, CancellationToken cancellationToken = default)
    {
        return await context.Equipments.Where(e => e.Type == type && e.Status == EquipmentStatus.Available)
            .ToListAsync();
    }

    public async Task<EquipmentAssignment> AddAssignmentAsync(EquipmentAssignment equipmentAssignment,
        CancellationToken cancellationToken = default)
    {
       
        await context.EquipmentAssignments.AddAsync(equipmentAssignment, cancellationToken);
        return equipmentAssignment;
        
    }

    public async Task<List<Equipment>> GetByEmployeeAsync(Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        return await context.Equipments.Include(e => e.Assignments)
            .Where(e => e.Assignments.Any(a => a.EmployeeId == employeeId)).ToListAsync();
    }

    public async Task<List<Equipment>> GetActiveEquipmentAsync(CancellationToken cancellationToken = default)
    {
        return await context.Equipments
            .Where(e => e.Status == EquipmentStatus.Assigned || e.Status == EquipmentStatus.Available).ToListAsync();
    }
}