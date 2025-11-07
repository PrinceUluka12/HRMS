using HRMS.Application.Features.Notifications.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.NotificationAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;


namespace HRMS.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<BaseResult<NotificationDto>>
    {
        public Guid? EmployeeId { get; set; } // null = broadcast
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string PayloadJson { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public class CreateNotificationHandler(INotificationRepository _repository, HRMS.Application.Interfaces.INotificationPublisher _publisher, IUnitOfWork unitOfWork) : IRequestHandler<CreateNotificationCommand, BaseResult<NotificationDto>>
    {
        public async Task<BaseResult<NotificationDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            NotificationType type = NotificationType.Info;
            Enum.TryParse<NotificationType>(request.Type ?? "Info", true, out type);


            var entity = new Notification(request.EmployeeId, request.Title, request.Message, type, request.PayloadJson, request.ExpiresAt);


            var added = await _repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();


            var dto = new NotificationDto
            {
                Id = added.Id,
                EmployeeId = added.EmployeeId,
                Title = added.Title,
                Message = added.Message,
                Type = added.Type.ToString().ToLower(),
                Status = added.Status.ToString().ToLower(),
                CreatedAt = added.CreatedAt,
                ExpiresAt = added.ExpiresAt,
                RemindersSent = added.RemindersSent,
                PayloadJson = added.PayloadJson
            };


            // Push to SignalR: targeted or broadcast
            if (added.IsBroadcast)
            {
                await _publisher.BroadcastAsync(dto);
            }
            else if (added.EmployeeId.HasValue)
            {
                await _publisher.PublishToEmployeeAsync(added.EmployeeId.Value, dto);
            }


            return BaseResult<NotificationDto>.Ok(dto);
        }
    }


}
