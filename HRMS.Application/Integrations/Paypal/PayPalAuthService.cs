using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HRMS.Application.Integrations.Paypal.Models;

namespace HRMS.Application.Integrations.Paypal;

public class PayPalAuthService(HttpClient httpClient)
    : IPayPalAuthService
{

    public async Task<string> GetAccessTokenAsync()
    {
        string _clientId = "AeQizpLRqwwZQxTHl4kn3eMtg-Zd-iyINgaKuNnrE18mZL30qDahxPC4oTwLWiwmy4fAe7ZBm-S-fz0q";
        string _clientSecret = "EJjRDyKCET2Mah3VfXQefRjxm7omt1cD8XqR5-H_d8Al_NmO5fdSyZXr7z_IaNsDckrMD47YuFy7qIMh";
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api-m.sandbox.paypal.com/v1/oauth2/token");
            
        // Basic Auth Header
        var credentials = Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            
        // Body - x-www-form-urlencoded
        request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get PayPal access token. StatusCode: {response.StatusCode}, Details: {error}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<PayPalTokenResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return tokenResponse.access_token;
    }
}