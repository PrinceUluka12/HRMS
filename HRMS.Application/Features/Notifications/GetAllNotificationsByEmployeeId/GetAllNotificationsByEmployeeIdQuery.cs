using HRMS.Application.Features.Notifications.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;

namespace HRMS.Application.Features.Notifications.GetAllNotificationsByEmployeeId;

public sealed record GetAllNotificationsByEmployeeIdQuery(Guid EmployeeId) :  IRequest<BaseResult<IEnumerable<NotificationDto>>>;

public class GetAllNotificationsByEmployeeIdQueryHandler(INotificationRepository notificationRepository) : IRequestHandler<GetAllNotificationsByEmployeeIdQuery, BaseResult<IEnumerable<NotificationDto>>>
{
    public async Task<BaseResult<IEnumerable<NotificationDto>>> Handle(GetAllNotificationsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
       var data =  await notificationRepository.GetByEmployeeIdIdAsync(request.EmployeeId, cancellationToken); 
       return data.OrderByDescending(n => n.Timestamp).Take(20)
           .Select(n => new NotificationDto
           {
               Id = n.Id,
               Message = n.Message,
               Timestamp = n.Timestamp
           }).ToList();
    }
}
