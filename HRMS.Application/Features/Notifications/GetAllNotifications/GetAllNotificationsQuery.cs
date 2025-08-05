using HRMS.Application.Features.Notifications.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;

namespace HRMS.Application.Features.Notifications.GetAllNotifications;

public record GetAllNotificationsQuery():IRequest<BaseResult<IEnumerable<NotificationDto>>>;

public class GetAllNotificationsQueryHandler(INotificationRepository notificationRepository) : IRequestHandler<GetAllNotificationsQuery , BaseResult<IEnumerable<NotificationDto>>>
{
    public async Task<BaseResult<IEnumerable<NotificationDto>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var data =  await notificationRepository.GetAllAsync();

        return data.OrderByDescending(n => n.Timestamp).Take(20)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                Timestamp = n.Timestamp
            }).ToList();
    }
}