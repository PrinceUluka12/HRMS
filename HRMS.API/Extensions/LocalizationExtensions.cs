using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace HRMS.API.Extensions;

public static class LocalizationExtensions
{
    /// <summary>
        /// Configures localization services by retrieving supported cultures from the configuration.
        /// </summary>
        /// <param name="services">The service collection to add localization support to.</param>
        /// <param name="configuration">The application configuration to retrieve supported cultures.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddCustomLocalization(this IServiceCollection services, IConfiguration configuration)
        {
            // Fetch supported cultures from configuration, ensuring a default list exists.
            var cultureCodes = configuration.GetSection("Localization:SupportedCultures").Get<List<string>>() ?? new List<string> { "en-US" };

            // Filter out invalid culture codes
            var supportedCultures = cultureCodes
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Select(code =>
                {
                    try { return new CultureInfo(code); }
                    catch (CultureNotFoundException) { return null; } // Ignore invalid cultures
                })
                .Where(culture => culture is not null)
                .ToArray();

            // Determine default request culture with a fallback.
            var defaultCultureCode = configuration["Localization:DefaultRequestCulture"] ?? "en-US";
            var defaultCulture = supportedCultures.FirstOrDefault(c => c.Name == defaultCultureCode) ?? new CultureInfo("en-US");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            return services;
        }

        /// <summary>
        /// Applies localization middleware to the application request pipeline.
        /// </summary>
        /// <param name="app">The application builder to configure middleware for localization.</param>
        /// <returns>The updated IApplicationBuilder.</returns>
        public static IApplicationBuilder UseCustomLocalization(this IApplicationBuilder app)
        {
            var localizationOptions = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            return app;
        }
}