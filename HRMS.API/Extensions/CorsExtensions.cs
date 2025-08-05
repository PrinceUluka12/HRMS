namespace HRMS.API.Extensions;

public static class CorsExtensions
{
    private const string CorsPolicyName = "WebClient";
    /// <summary>
    /// Configures the CORS policy using settings from configuration.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the CORS configuration to.</param>
    /// <param name="configuration">The IConfiguration instance to retrieve CORS settings from.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddConfiguredCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        return services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, builder =>
            {
                builder.WithOrigins("https://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod(); // Only use this if explicit origins are defined
            });
        });
    }
    
    /// <summary>
    /// Applies the configured CORS policy to the application middleware pipeline.
    /// </summary>
    /// <param name="app">The IApplicationBuilder to configure CORS for.</param>
    /// <returns>The updated IApplicationBuilder.</returns>
    public static IApplicationBuilder UseConfiguredCors(this IApplicationBuilder app)
    {
        return app.UseCors(CorsPolicyName);
    }
}