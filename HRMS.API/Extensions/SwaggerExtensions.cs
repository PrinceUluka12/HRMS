using Microsoft.OpenApi.Models;

namespace HRMS.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithAzureAdSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HRMS API", Version = "v1" });
            
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{configuration["AzureAd:Instance"]}/{configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri($"{configuration["AzureAd:Instance"]}/{configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { $"api://{configuration["AzureAd:ClientId"]}/access_as_user", "Access the API as a user" }
                        }
                    }
                }
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    new[] { $"api://{configuration["AzureAd:ClientId"]}/access_as_user" }
                }
            });
        });

        return services;
    }
}