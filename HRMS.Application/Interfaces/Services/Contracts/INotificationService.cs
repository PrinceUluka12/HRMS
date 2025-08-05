namespace HRMS.Application.Interfaces.Services.Contracts;

public interface INotificationService
{
    Task BroadcastAsync(string message, Guid EmployeeId);
    Task NotifyUserAsync(Guid EmployeeId, string message);
}