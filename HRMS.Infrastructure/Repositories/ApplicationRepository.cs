using HRMS.Application.Interfaces.Repositories;
using HRMS.Infrastructure.Persistence;

namespace HRMS.Infrastructure.Repositories;

public class ApplicationRepository(ApplicationDbContext context) :GenericRepository<Domain.Aggregates.RecruitmentAggregates.Application>(context) ,IApplicationRepository
{
    
}