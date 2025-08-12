namespace HRMS.Application.Integrations.HyperWallet.Models;

public class HyperwalletUserRequest
{
    public string clientUserId { get; set; }
    public string profileType { get; set; } = "INDIVIDUAL";
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string country { get; set; } = "US";
    public string currency { get; set; } = "USD";
    public string programToken { get; set; }
}