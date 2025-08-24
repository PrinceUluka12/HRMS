using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.RecruitmentAggregates;

namespace HRMS.Application.Interfaces.Repositories;

public interface IJobVacancyRepository : IGenericRepository<JobVacancy>
{
    Task<JobVacancy> GetAllWithApplicationsAsync(Guid Id);
    Task<List<HRMS.Domain.Aggregates.RecruitmentAggregates.Application>> GetAllApplicationsByVacancyId(Guid Id);
    
}