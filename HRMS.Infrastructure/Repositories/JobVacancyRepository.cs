using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.RecruitmentAggregates;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class JobVacancyRepository(ApplicationDbContext context) : GenericRepository<JobVacancy>(context), IJobVacancyRepository
{
    public async Task<JobVacancy> GetAllWithApplicationsAsync(Guid Id)
    {
        return await  context.JobVacancies.Include(a => a.Applications).FirstOrDefaultAsync(e => e.Id == Id);
    }

    public async Task<List<Domain.Aggregates.RecruitmentAggregates.Application>> GetAllApplicationsByVacancyId(Guid Id)
    {
        return await context.Applications.Where(j => j.JobVacancyId == Id).ToListAsync();
    }
}