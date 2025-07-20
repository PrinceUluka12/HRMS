using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Aggregates.PayrollAggregate;

namespace HRMS.Application.Interfaces.Services.Contracts;

public interface IEmailService
{
    Task SendPayslipEmailAsync(string recipientEmail, Payroll payroll);
    Task SendWelcomeEmailAsync(string recipientEmail, Employee employee);
    Task SendLeaveApprovalEmailAsync(string recipientEmail, LeaveRequest leaveRequest);
}