namespace HRMS.Domain.Interfaces;

public interface IDomainEvent
{
}
public interface IHasDomainEvents
{
    List<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}