namespace HRMS.Application.Integrations.Paypal.Models;

public class PayPalPayoutResponse
{
    public BatchHeader batch_header { get; set; }
    public List<Link> links { get; set; }
}

public class BatchHeader
{
    public string payout_batch_id { get; set; }
    public string batch_status { get; set; }
    public SenderBatchHeader sender_batch_header { get; set; }
}

public class Link
{
    public string href { get; set; }
    public string rel { get; set; }
    public string method { get; set; }
    public string encType { get; set; }
}