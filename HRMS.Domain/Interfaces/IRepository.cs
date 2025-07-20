using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Interfaces;

public interface IRepository<T> where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}