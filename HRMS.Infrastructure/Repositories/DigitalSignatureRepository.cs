using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.DigitalSignatureAggregate;
using HRMS.Domain.Aggregates.RecruitmentAggregates;
using HRMS.Domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class DigitalSignatureRepository(ApplicationDbContext context) : GenericRepository<DigitalSignature>(context), IDigitalSignatureRepository
    {
        public async Task<IEnumerable<DigitalSignature>> GetByEmployeeAsync(Guid employeeId)
        {
            return await context.DigitalSignatures.Where(x => x.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<IEnumerable<DigitalSignature>> GetPendingByEmployeeAsync(Guid employeeId)
        {
            return await context.DigitalSignatures.Where(x => x.EmployeeId == employeeId && x.Status == SignatureStatus.Pending).ToListAsync();
        }
    }
}
