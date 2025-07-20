using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.PayrollAggregate;

public class Payroll : Entity<Guid>, IAggregateRoot
{
    // Core properties
    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }
    public DateTime PayPeriodStart { get; private set; }
    public DateTime PayPeriodEnd { get; private set; }
    public decimal GrossSalary { get; private set; }
    public decimal TaxDeductions { get; private set; }
    public decimal BenefitsDeductions { get; private set; }
    public bool HasCPPExemption { get; private set; }
    public bool HasEIExemption {get; private set;}
    public decimal NetSalary { get; private set; }
    public PayrollStatus Status { get; private set; }
    public DateTime ProcessedDate { get; private set; }
    public string ProcessedBy { get; private set; }
    public string? PaymentReference { get; private set; }
    public DateTime? PaymentDate { get; private set; }
    
    // Collections
    private readonly List<PayrollItem> _items = new();
    public IReadOnlyCollection<PayrollItem> Items => _items.AsReadOnly();

    // Private constructor for EF Core
    private Payroll() { }

    public Payroll(
        Guid employeeId,
        DateTime payPeriodStart,
        DateTime payPeriodEnd,
        decimal grossSalary,
        bool hasCPPExemption,
        bool hasEIExemption,
        decimal taxDeductions,
        decimal benefitsDeductions,
        string processedBy)
    {
        EmployeeId = employeeId;
        PayPeriodStart = payPeriodStart;
        PayPeriodEnd = payPeriodEnd;
        GrossSalary = grossSalary;
        HasCPPExemption = hasCPPExemption;
        HasEIExemption = hasEIExemption;
        TaxDeductions = taxDeductions;
        BenefitsDeductions = benefitsDeductions;
        NetSalary = grossSalary - taxDeductions - benefitsDeductions;
        Status = PayrollStatus.Processed;
        ProcessedDate = DateTime.UtcNow;
        ProcessedBy = processedBy ?? throw new ArgumentNullException(nameof(processedBy));

        Validate();
    }

    // Domain methods
    public void AddItem(PayrollItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (Status != PayrollStatus.Draft && Status != PayrollStatus.Processed)
            throw new DomainException("Items can only be added to draft or processed payrolls");

        _items.Add(item);
        RecalculateTotals();
    }

    public void RemoveItem(Guid itemId)
    {
        if (Status != PayrollStatus.Draft)
            throw new DomainException("Items can only be removed from draft payrolls");

        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _items.Remove(item);
            RecalculateTotals();
        }
    }

    public void Approve(string approvedBy)
    {
        if (Status != PayrollStatus.Draft)
            throw new DomainException("Only draft payrolls can be approved");

        Status = PayrollStatus.Approved;
        ModifiedBy(approvedBy);
    }

    public void Process(string processedBy)
    {
        if (Status != PayrollStatus.Approved)
            throw new DomainException("Only approved payrolls can be processed");

        Status = PayrollStatus.Processed;
        ProcessedDate = DateTime.UtcNow;
        ProcessedBy = processedBy ?? throw new ArgumentNullException(nameof(processedBy));
    }

    public void MarkAsPaid(string paymentReference, DateTime paymentDate, string markedBy)
    {
        if (Status != PayrollStatus.Processed && Status != PayrollStatus.SubmittedForPayment)
            throw new DomainException("Only processed payrolls can be marked as paid");

        Status = PayrollStatus.Paid;
        PaymentReference = paymentReference ?? throw new ArgumentNullException(nameof(paymentReference));
        PaymentDate = paymentDate;
        ModifiedBy(markedBy);
    }

    public void MarkAsFailed(string reason, string markedBy)
    {
        if (Status != PayrollStatus.SubmittedForPayment)
            throw new DomainException("Only submitted payrolls can be marked as failed");

        Status = PayrollStatus.PaymentFailed;
        AddSystemNote($"Payment failed: {reason}");
        ModifiedBy(markedBy);
    }
    

    public void Cancel(string cancellationReason, string cancelledBy)
    {
        if (Status == PayrollStatus.Paid)
            throw new DomainException("Paid payrolls cannot be cancelled");

        Status = PayrollStatus.Cancelled;
        AddSystemNote($"Cancelled: {cancellationReason}");
        ModifiedBy(cancelledBy);
    }

    // Helper methods
    private void RecalculateTotals()
    {
        GrossSalary = _items
            .Where(i => i.Type == PayrollItemType.Salary)
            .Sum(i => i.Amount);

        TaxDeductions = _items
            .Where(i => i.Type == PayrollItemType.Tax)
            .Sum(i => i.Amount);

        BenefitsDeductions = _items
            .Where(i => i.Type == PayrollItemType.BenefitDeduction)
            .Sum(i => i.Amount);

        NetSalary = GrossSalary - TaxDeductions - BenefitsDeductions;
    }

    private void Validate()
    {
        if (PayPeriodStart >= PayPeriodEnd)
            throw new DomainException("Pay period start must be before end date");

        if (GrossSalary < 0)
            throw new DomainException("Gross salary cannot be negative");

        if (TaxDeductions < 0 || BenefitsDeductions < 0)
            throw new DomainException("Deductions cannot be negative");

        if (NetSalary < 0)
            throw new DomainException("Net salary cannot be negative");
    }

    private void ModifiedBy(string modifiedBy)
    {
        // In a full implementation, this would track who made the change
    }

    private void AddSystemNote(string note)
    {
        // In a full implementation, this would add to an audit log
    }
}