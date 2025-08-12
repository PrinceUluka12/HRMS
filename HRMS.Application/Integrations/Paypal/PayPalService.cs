using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HRMS.Application.Integrations.Paypal.Models;

namespace HRMS.Application.Integrations.Paypal;

public class PayPalService(HttpClient httpClient, IPayPalAuthService authService) : IPayPalService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IPayPalAuthService _authService = authService;


    public async Task<PayPalPayoutResponse> SendPayoutAsync(PayPalPayoutRequest request)
    {
        var accessToken = await _authService.GetAccessTokenAsync();

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api-m.sandbox.paypal.com/v1/payments/payouts");
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(httpRequest);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"PayPal payout failed: {content}");
        }

        var result = JsonSerializer.Deserialize<PayPalPayoutResponse>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return result;
    }
}