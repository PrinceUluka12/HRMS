using HRMS.Application.Features.Notifications.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Notifications.Queries
{
    public class GetEmployeeNotificationsQuery : IRequest<BaseResult<IEnumerable<NotificationDto>>>
    {
        public Guid EmployeeId { get; set; }
        public int Take { get; set; } = 50;
        public int Skip { get; set; } = 0;
    }

    public class GetEmployeeNotificationsHandler(INotificationRepository _repository) : IRequestHandler<GetEmployeeNotificationsQuery, BaseResult<IEnumerable<NotificationDto>>>
    {
        public async Task<BaseResult<IEnumerable<NotificationDto>>> Handle(GetEmployeeNotificationsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetByEmployeeAsync(request.EmployeeId, request.Take, request.Skip);
            var data =  items.Select(i => new NotificationDto
            {
                Id = i.Id,
                EmployeeId = i.EmployeeId,
                Title = i.Title,
                Message = i.Message,
                Type = i.Type.ToString().ToLower(),
                Status = i.Status.ToString().ToLower(),
                CreatedAt = i.CreatedAt,
                ExpiresAt = i.ExpiresAt,
                RemindersSent = i.RemindersSent,
                PayloadJson = i.PayloadJson
            });

            return BaseResult<IEnumerable<NotificationDto>>.Ok(data);
        }
    }
}
