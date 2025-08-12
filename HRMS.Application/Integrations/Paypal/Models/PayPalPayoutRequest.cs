namespace HRMS.Application.Integrations.Paypal.Models;

public class PayPalPayoutRequest
{
    public SenderBatchHeader sender_batch_header { get; set; }
    public List<PayoutItem> items { get; set; }
}

public class SenderBatchHeader
{
    public string sender_batch_id { get; set; } =  new Guid().ToString();
    public string recipient_type { get; set; } // EMAIL or PHONE
    public string email_subject { get; set; }
    public string email_message { get; set; }
}

public class PayoutItem
{
    public Amount amount { get; set; }
    public string sender_item_id { get; set; }
    public string recipient_wallet { get; set; } // PAYPAL
    public string receiver { get; set; } // Email or phone
    public string note { get; set; }
    public string recipient_type { get; set; } // Optional: EMAIL or PHONE
}

public class Amount
{
    public string value { get; set; }
    public string currency { get; set; }
}