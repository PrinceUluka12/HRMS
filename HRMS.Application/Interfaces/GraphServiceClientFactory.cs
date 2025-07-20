using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace HRMS.Application.Interfaces;

public class GraphServiceClientFactory(
    IConfiguration configuration,
    ITokenAcquisition tokenAcquisition) 
{
    public GraphServiceClient CreateClient()
    {
        var tenantId = configuration["AzureAd:TenantId"];
        var clientId = configuration["AzureAd:ClientId"];
        var clientSecret = configuration["AzureAd:ClientSecret"];

        var scopes = new[] { "https://graph.microsoft.com/.default" };

        var clientSecretCredential = new ClientSecretCredential(
            tenantId,
            clientId,
            clientSecret);

        return new GraphServiceClient(clientSecretCredential, scopes);
    }
}