namespace HRMS.Application.Integrations.Paypal;

public interface IPayPalAuthService
{
    Task<string> GetAccessTokenAsync();
}