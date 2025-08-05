using System.Text;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Aggregates.PayrollAggregate;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Me.SendMail;

namespace HRMS.Application.Interfaces.Services;

public class EmailService(
    GraphServiceClient graphServiceClient,
    ILogger<EmailService> logger)
    : IEmailService
{
    public async Task SendPayslipEmailAsync(string recipientEmail, Payroll payroll)
    {
        if (string.IsNullOrWhiteSpace(recipientEmail))
            throw new ArgumentException("Recipient email cannot be null or empty.", nameof(recipientEmail));

        if (payroll == null)
            throw new ArgumentNullException(nameof(payroll));

        try
        {
            var pdfAttachment = new FileAttachment
            {
                Name = $"Payslip_{payroll.PayPeriodStart:yyyyMMdd}-{payroll.PayPeriodEnd:yyyyMMdd}.pdf",
                ContentType = "application/pdf",
                ContentBytes = GeneratePayslipPdf(payroll),
            };

            var message = new Message
            {
                Subject = $"Your Payslip: {payroll.PayPeriodStart:yyyy-MM-dd} to {payroll.PayPeriodEnd:yyyy-MM-dd}",
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = GeneratePayslipHtml(payroll)
                },
                ToRecipients = new List<Recipient>
                {
                    new()
                    {
                        EmailAddress = new EmailAddress { Address = recipientEmail }
                    }
                },
                Attachments = new List<Attachment> { pdfAttachment }
            };

            var sendMailRequest = new SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            };

            await graphServiceClient.Me.SendMail.PostAsync(sendMailRequest);
            logger.LogInformation("Payslip email sent to {Email}", recipientEmail);
        }
        catch (ServiceException ex)
        {
            logger.LogError(ex, "Error sending payslip email to {Email}", recipientEmail);
            throw;
        }
    }

    public async Task SendWelcomeEmailAsync(string recipientEmail, Employee employee)
    {
        if (string.IsNullOrWhiteSpace(recipientEmail))
            throw new ArgumentException("Recipient email cannot be null or empty.", nameof(recipientEmail));

        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        try
        {
            var body = new StringBuilder();
            body.AppendLine("<html><body>");
            body.AppendLine($"<h2>Welcome to the Company, {employee.Name.FirstName}!</h2>");
            body.AppendLine("<p>We are excited to have you on board.</p>");
            body.AppendLine("</body></html>");

            var message = new Message
            {
                Subject = "Welcome to the Team!",
                Body = new ItemBody { ContentType = BodyType.Html, Content = body.ToString() },
                ToRecipients = new List<Recipient>
                {
                    new() { EmailAddress = new EmailAddress { Address = recipientEmail } }
                }
            };

            await graphServiceClient.Me.SendMail.PostAsync(new SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            });

            logger.LogInformation("Welcome email sent to {Email}", recipientEmail);
        }
        catch (ServiceException ex)
        {
            logger.LogError(ex, "Error sending welcome email to {Email}", recipientEmail);
            throw;
        }
    }

    public async Task SendLeaveApprovalEmailAsync(string recipientEmail, LeaveRequest leaveRequest)
    {
        if (string.IsNullOrWhiteSpace(recipientEmail))
            throw new ArgumentException("Recipient email cannot be null or empty.", nameof(recipientEmail));

        if (leaveRequest == null)
            throw new ArgumentNullException(nameof(leaveRequest));

        try
        {
            var content = new StringBuilder();
            content.AppendLine("<html><body>");
            content.AppendLine($"<h2>Leave Approved</h2>");
            content.AppendLine($"<p>Your leave request from <b>{leaveRequest.StartDate:yyyy-MM-dd}</b> to <b>{leaveRequest.EndDate:yyyy-MM-dd}</b> has been <strong>approved</strong>.</p>");
            content.AppendLine($"<p>Type: {leaveRequest.Type}</p>");
            content.AppendLine("</body></html>");

            var message = new Message
            {
                Subject = "Leave Approval Notification",
                Body = new ItemBody { ContentType = BodyType.Html, Content = content.ToString() },
                ToRecipients = new List<Recipient>
                {
                    new() { EmailAddress = new EmailAddress { Address = recipientEmail } }
                }
            };

            await graphServiceClient.Me.SendMail.PostAsync(new SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            });

            logger.LogInformation("Leave approval email sent to {Email}", recipientEmail);
        }
        catch (ServiceException ex)
        {
            logger.LogError(ex, "Error sending leave approval email to {Email}", recipientEmail);
            throw;
        }
    }

    private string GeneratePayslipHtml(Payroll payroll)
    {
        return $"""
        <html>
            <body style="font-family: Arial, sans-serif;">
            <body>
                <h1>Payslip</h1>
                <p>Pay Period: {payroll.PayPeriodStart:yyyy-MM-dd} to {payroll.PayPeriodEnd:yyyy-MM-dd}</p>
                <p>Gross Salary: {payroll.GrossSalary:C}</p>
                <p>Tax Deductions: {payroll.TaxDeductions:C}</p>
                <p>Benefits Deductions: {payroll.BenefitsDeductions:C}</p>
                <h3>Net Salary: {payroll.NetSalary:C}</h3>
                <p>Please see attached PDF for detailed breakdown.</p>
            </body>
        </html>
        """;
    }

    private byte[] GeneratePayslipPdf(Payroll payroll)
    {
        // Placeholder logic; consider using QuestPDF, DinkToPDF, or iTextSharp for real implementation
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.WriteLine($"Payslip for {payroll.PayPeriodStart:yyyy-MM-dd} to {payroll.PayPeriodEnd:yyyy-MM-dd}");
        writer.WriteLine($"Gross Salary: {payroll.GrossSalary:C}");
        writer.WriteLine($"Tax Deductions: {payroll.TaxDeductions:C}");
        writer.WriteLine($"Benefits Deductions: {payroll.BenefitsDeductions:C}");
        writer.WriteLine($"Net Salary: {payroll.NetSalary:C}");
        writer.Flush();
        return stream.ToArray();
    }
}
