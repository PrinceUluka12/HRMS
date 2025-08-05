using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Domain.Aggregates.EmployeeAggregate.Events;

public class EmployeeCreatedDomainEvent : IDomainEvent, INotification
{
    public Guid EmployeeId { get; }

    public EmployeeCreatedDomainEvent(Guid employeeId)
    {
        EmployeeId = employeeId;
    }
    
}