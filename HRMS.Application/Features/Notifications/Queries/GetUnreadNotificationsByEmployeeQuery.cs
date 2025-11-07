using HRMS.Application.Features.Notifications.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Notifications.Queries
{
    public record GetUnreadNotificationsByEmployeeQuery(Guid EmployeeId) : IRequest<IEnumerable<NotificationDto>>;

    public class GetUnreadNotificationsByEmployeeHandler(IEmployeeRepository employeeRepository, INotificationRepository _repository) : IRequestHandler<GetUnreadNotificationsByEmployeeQuery, IEnumerable<NotificationDto>>
    {
        
        public async Task<IEnumerable<NotificationDto>> Handle(GetUnreadNotificationsByEmployeeQuery request, CancellationToken cancellationToken)
        {
            Guid empId = await GetEmployeeIdFromAzureId(request.EmployeeId);

            var notifications = await _repository.GetUnreadByEmployeeAsync(empId);
            var dtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                EmployeeId = n.EmployeeId,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type.ToString().ToLower(),
                Status = n.Status.ToString().ToLower(),
                CreatedAt = n.CreatedAt,
                ExpiresAt = n.ExpiresAt,
                RemindersSent = n.RemindersSent,
                PayloadJson = n.PayloadJson
            });
            return dtos;
        }

        private async Task<Guid> GetEmployeeIdFromAzureId(Guid AzureId)
        {
          var data =  await  employeeRepository.GetByAzureAdIdAsync(AzureId);

            return data.Id;
        }
    }
}
