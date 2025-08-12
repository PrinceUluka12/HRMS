namespace HRMS.Application.Integrations.HyperWallet.Models;

public class HyperwalletBankAccountRequest
{
    public string transferMethodCountry { get; set; } = "US";
    public string transferMethodCurrency { get; set; } = "USD";
    public string type { get; set; } = "BANK_ACCOUNT";
    public string bankAccountId { get; set; }
    public string branchId { get; set; }
    public string bankName { get; set; }
    public string accountHolderName { get; set; }
    public string profileType { get; set; } = "INDIVIDUAL";
    public string userToken { get; set; }
}