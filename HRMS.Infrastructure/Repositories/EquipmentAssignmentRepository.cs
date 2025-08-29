using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class EquipmentAssignmentRepository(ApplicationDbContext context): GenericRepository<EquipmentAssignment>(context),IEquipmentAssignmentRepository
{
    public async Task<List<EquipmentAssignment>> GetByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await context.EquipmentAssignments.Where(e => e.EmployeeId == employeeId).ToListAsync();
    }

   

    public async Task<List<EquipmentAssignment>> GetActiveEquipmentAsync(CancellationToken cancellationToken = default)
    {
        return await context.EquipmentAssignments.Where(e => e.Status == EquipmentAssignmentStatus.Assigned)
            .ToListAsync();
    }
}