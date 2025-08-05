namespace HRMS.Domain.Aggregates.NotificationAggregate;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public bool IsRead { get; set; } = false;
    public Guid EmployeeId { get; set; }
}