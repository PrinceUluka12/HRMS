using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.RecruitmentAggregates;
using HRMS.Infrastructure.Persistence;

namespace HRMS.Infrastructure.Repositories;

public class CandidateRepository(ApplicationDbContext context) :GenericRepository<Candidate>(context), ICandidateRepository
{
    
}