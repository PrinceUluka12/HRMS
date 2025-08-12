using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HRMS.Application.Integrations.HyperWallet.Models;

namespace HRMS.Application.Integrations.HyperWallet;

public class HyperwalletService : IHyperwalletService
{
    private readonly HttpClient _httpClient;
    private readonly HyperwalletConfiguration _config;

    public HyperwalletService(HyperwalletConfiguration config)
    {
        _config = config;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(config.BaseUrl.TrimEnd('/') + "/rest/v4/")
        };

        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{config.Username}:{config.Password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
    }

    public async Task<string> CreateUserAsync(HyperwalletUserRequest user)
    {
        user.programToken = _config.ProgramToken;
        var response = await PostAsync("users", user);
        return JsonDocument.Parse(response).RootElement.GetProperty("token").GetString();
    }

    public async Task<string> CreateBankAccountAsync(HyperwalletBankAccountRequest bank)
    {
        var response = await PostAsync("bank-accounts", bank);
        return JsonDocument.Parse(response).RootElement.GetProperty("token").GetString();
    }

    public async Task<string> MakePaymentAsync(HyperwalletPaymentRequest payment)
    {
        payment.programToken = _config.ProgramToken;
        var response = await PostAsync("payments", payment);
        return JsonDocument.Parse(response).RootElement.GetProperty("status").GetString();
    }

    public async Task<string> PostAsync(string endpoint, object body)
    {
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, content);

        var responseBody = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Hyperwallet API Error: {response.StatusCode} - {responseBody}");

        return responseBody;
    }
}