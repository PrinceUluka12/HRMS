using HRMS.Application.Integrations.HyperWallet.Models;

namespace HRMS.Application.Integrations.HyperWallet;

public interface IHyperwalletService
{
    Task<string> CreateUserAsync(HyperwalletUserRequest user);
    Task<string> CreateBankAccountAsync(HyperwalletBankAccountRequest bank);
    Task<string> MakePaymentAsync(HyperwalletPaymentRequest payment);
    Task<string> PostAsync(string endpoint, object body);
}