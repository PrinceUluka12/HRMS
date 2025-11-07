using HRMS.Domain.Enums;

namespace HRMS.Domain.Aggregates.NotificationAggregate;

public sealed class Notification
{
    public Guid Id { get; private set; }
    public Guid? EmployeeId { get; private set; } // null => broadcast to all
    public string Title { get; private set; }
    public string Message { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public int RemindersSent { get; private set; }
    public string PayloadJson { get; private set; } // optional extra data


    protected Notification() { }

    public Notification(Guid? employeeId, string title, string message, NotificationType type = NotificationType.Info, string payloadJson = null, DateTime? expiresAt = null)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Message = message ?? string.Empty;
        Type = type;
        Status = NotificationStatus.Unread;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        RemindersSent = 0;
        PayloadJson = payloadJson;
    }


    public bool IsBroadcast => !EmployeeId.HasValue;


    public void MarkAsRead()
    {
        Status = NotificationStatus.Read;
    }


    public void MarkAsArchived()
    {
        Status = NotificationStatus.Archived;
    }


    public void IncrementReminders()
    {
        RemindersSent++;
    }
}