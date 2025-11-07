using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;


namespace HRMS.Application.Features.Notifications.Commands
{
    public class MarkNotificationReadCommand : IRequest<BaseResult>
    {
        public Guid NotificationId { get; set; }
        public Guid EmployeeId { get; set; }
    }

    public class MarkNotificationReadHandler(INotificationRepository _repository, IUnitOfWork unitOfWork, IEmployeeRepository employeeRepository) : IRequestHandler<MarkNotificationReadCommand, BaseResult>
    {
        public async Task<BaseResult> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.NotificationId);
            if (item == null) throw new KeyNotFoundException("Notification not found");

            Guid EmpId = await GetEmployeeIdFromAzureId(request.EmployeeId);

            if (item.EmployeeId != EmpId) throw new UnauthorizedAccessException("Cannot mark other user's notification");


            item.MarkAsRead();
            await _repository.Update(item);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResult.Ok();
        }
        private async Task<Guid> GetEmployeeIdFromAzureId(Guid AzureId)
        {
            var data = await employeeRepository.GetByAzureAdIdAsync(AzureId);

            return data.Id;
        }
    }
}
