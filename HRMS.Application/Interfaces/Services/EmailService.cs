using System.Text;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Aggregates.PayrollAggregate;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Me.SendMail;
using Microsoft.Graph.Models;

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
                //ODataType = "#microsoft.graph.fileAttachment"
            };

            var message = new Message
            {
                Subject = $"Your Payslip for {payroll.PayPeriodStart:yyyy-MM-dd} to {payroll.PayPeriodEnd:yyyy-MM-dd}",
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = GeneratePayslipHtml(payroll)
                },
                ToRecipients = new List<Recipient>
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = recipientEmail
                        }
                    }
                },
                Attachments = new List<Attachment> {pdfAttachment}
            };

            var sendMailRequest = new SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            };

            await graphServiceClient.Me
                .SendMail
                .PostAsync(sendMailRequest)
                .ConfigureAwait(false);
        }
        catch (ServiceException ex)
        {
            logger.LogError(ex, "Error sending payslip email to {Email}", recipientEmail);
            throw;
        }
    }


    public async Task SendWelcomeEmailAsync(string recipientEmail, Employee employee)
    {
        throw new NotImplementedException();
    }

    public async Task SendLeaveApprovalEmailAsync(string recipientEmail, LeaveRequest leaveRequest)
    {
        throw new NotImplementedException();
    }
    
    private string GeneratePayslipHtml(Payroll payroll)
    {
        var html = new StringBuilder();
        html.AppendLine("<html>");
        html.AppendLine("<head><style>body { font-family: Arial, sans-serif; }</style></head>");
        html.AppendLine("<body>");
        html.AppendLine("<h1>Payslip</h1>");
        html.AppendLine($"<p>Pay Period: {payroll.PayPeriodStart:yyyy-MM-dd} to {payroll.PayPeriodEnd:yyyy-MM-dd}</p>");
        html.AppendLine($"<p>Gross Salary: {payroll.GrossSalary:C}</p>");
        html.AppendLine($"<p>Tax Deductions: {payroll.TaxDeductions:C}</p>");
        html.AppendLine($"<p>Benefits Deductions: {payroll.BenefitsDeductions:C}</p>");
        html.AppendLine($"<h3>Net Salary: {payroll.NetSalary:C}</h3>");
        html.AppendLine("<p>Please see attached PDF for detailed breakdown.</p>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");
        
        return html.ToString();
    }

    private byte[] GeneratePayslipPdf(Payroll payroll)
    {
        // In a real implementation, use a PDF generation library like iTextSharp or QuestPDF
        // This is a simplified placeholder
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