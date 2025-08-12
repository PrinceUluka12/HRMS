using HRMS.Application.Integrations.Paypal.Models;

namespace HRMS.Application.Integrations.Paypal;

public interface IPayPalService
{
    Task<PayPalPayoutResponse> SendPayoutAsync(PayPalPayoutRequest request);
}