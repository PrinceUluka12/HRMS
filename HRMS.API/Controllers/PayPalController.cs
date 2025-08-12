using HRMS.Application.Integrations.HyperWallet;
using HRMS.Application.Integrations.HyperWallet.Models;
using HRMS.Application.Integrations.Paypal;
using HRMS.Application.Integrations.Paypal.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayPalController(IPayPalAuthService palAuthService, IPayPalService palService, IHyperwalletService service): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetToken()
    {
        var result = await palAuthService.GetAccessTokenAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Pay(PayPalPayoutRequest request)
    {
        var result  = await palService.SendPayoutAsync(request);
        return Ok(result);
    }

    [HttpPost ("TryHyperWallet")]
    public async Task<IActionResult> TryHyperWallet()
    {
        var userToken = await service.CreateUserAsync(new HyperwalletUserRequest
        {
            clientUserId = $"user-{Guid.NewGuid()}",
            firstName = "Jane",
            lastName = "Doe",
            email = "jane.doe@example.com"
        });
        
        var bankToken = await service.CreateBankAccountAsync(new HyperwalletBankAccountRequest
        {
            userToken = userToken,
            bankAccountId = "123456789", // Routing Number
            branchId = "021000021",      // Bank Code (e.g., routing)
            bankName = "Bank of America",
            accountHolderName = "Jane Doe"
        });
        
        var paymentStatus = await service.MakePaymentAsync(new HyperwalletPaymentRequest
        {
            destinationToken = bankToken,
            clientPaymentId = $"payment-{Guid.NewGuid()}",
            amount = "50.00"
        });
        
        return Ok(paymentStatus);
    }
}