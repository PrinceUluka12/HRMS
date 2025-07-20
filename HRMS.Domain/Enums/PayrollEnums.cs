namespace HRMS.Domain.Enums;

public enum PayrollStatus
{
    // Initial draft state before finalization
    Draft = 0,
    
    // Approved by manager/supervisor
    Approved = 1,
    
    // Processed by payroll system (calculations complete)
    Processed = 2,
    
    // Sent to bank for payment processing
    SubmittedForPayment = 3,
    
    // Funds successfully deposited/paid
    Paid = 4,
    
    // Payment failed (bank rejection, etc.)
    PaymentFailed = 5,
    
    // Payroll has been cancelled
    Cancelled = 6,
    
    // Adjustment made after processing
    Adjusted = 7,
    
    // Pending tax verification
    PendingTaxReview = 8,
    
    // On hold due to issues
    OnHold = 9,
    
    // Reversed/rolled back
    Reversed = 10
}