namespace HRMS.Application.Features.Notifications.Dtos;

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid? EmployeeId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int RemindersSent { get; set; }
    public string PayloadJson { get; set; }
}