using HRMS.Application.Common.Interfaces;

namespace HRMS.Application.Interfaces.Repositories;

public interface IApplicationRepository : IGenericRepository<HRMS.Domain.Aggregates.RecruitmentAggregates.Application>
{
    
}