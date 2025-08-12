namespace HRMS.Application.Integrations.HyperWallet.Models;

public class HyperwalletPaymentRequest
{
    public string destinationToken { get; set; }
    public string clientPaymentId { get; set; }
    public string amount { get; set; }
    public string currency { get; set; } = "USD";
    public string purpose { get; set; } = "OTHER";
    public string programToken { get; set; }
}