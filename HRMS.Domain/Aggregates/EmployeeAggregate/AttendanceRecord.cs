using HRMS.Domain.Enums;

namespace HRMS.Domain.Aggregates.EmployeeAggregate;

public class AttendanceRecord
{
    public Guid Id { get; private set; } = Guid.NewGuid(); // Primary key

    public DateTime Date { get; private set; }
    public TimeSpan? ClockInTime { get; private set; }
    public TimeSpan? ClockOutTime { get; private set; }
    public AttendanceStatus Status { get; private set; }
    public string? Notes { get; private set; }

    // EF constructor
    private AttendanceRecord() { }

    public AttendanceRecord(DateTime date, TimeSpan? clockInTime, TimeSpan? clockOutTime, AttendanceStatus status, string? notes)
    {
        Date = date;
        ClockInTime = clockInTime;
        ClockOutTime = clockOutTime;
        Status = status;
        Notes = notes;
    }
}
