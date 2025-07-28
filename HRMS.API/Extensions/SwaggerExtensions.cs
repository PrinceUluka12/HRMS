using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HRMS.API.Extensions;

public static class SwaggerExtensions
{
    
    /// <summary>
    /// Configures Swagger UI to display API documentation with versioning.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <returns>The modified application builder instance.</returns>
    public static IApplicationBuilder UseSwaggerWithVersioning(this IApplicationBuilder app)
    {
        // Retrieve the API version description provider from the DI container
        IServiceProvider services = app.ApplicationServices;
        var provider = services.GetRequiredService<IApiVersionDescriptionProvider>();

        // Enable Swagger middleware to serve the OpenAPI JSON specification
        app.UseSwagger();

        // Configure Swagger UI to display endpoints for all API versions
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant()); // Label API versions in uppercase
            }
        });

        return app;
    }
    
      /// <summary>
        /// Adds Swagger documentation services with API versioning support.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddSwaggerWithVersioning(this IServiceCollection services)
        {
            // Configure API versioning
            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0); // Default API version is 1.0
                setup.AssumeDefaultVersionWhenUnspecified = true; // Assume default version if none is specified
                setup.ReportApiVersions = true; // Include API version information in response headers
            });

            // Configure API Explorer to support versioning
            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV"; // Format versions as "v1", "v2", etc.
                setup.SubstituteApiVersionInUrl = true; // Replace API version placeholders in route templates
            });

            // Register Swagger generation service
            services.AddSwaggerGen(setup =>
            {
                // Define JWT authentication scheme for Swagger
                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input 'Bearer {your token}' to authenticate."
                });

                // Apply authentication scheme to all API endpoints
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    }
                });
            });

            // Register dynamic Swagger configuration options
            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }
      
        /// <summary>
        /// Configures Swagger options dynamically based on API versioning.
        /// </summary>
        public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureNamedOptions<SwaggerGenOptions>
        {
            /// <summary>
            /// Configures Swagger documentation for each API version.
            /// </summary>
            /// <param name="options">The SwaggerGen options to configure.</param>
            public void Configure(SwaggerGenOptions options)
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var info = new OpenApiInfo()
                    {
                        Title = Assembly.GetCallingAssembly().GetName().Name, // Use assembly name as API title
                        Version = description.ApiVersion.ToString() // Set the API version
                    };

                    // Indicate if this API version is deprecated
                    if (description.IsDeprecated)
                        info.Description += "This API version has been deprecated.";

                    options.SwaggerDoc(description.GroupName, info); // Register API version in Swagger
                }
            }

            /// <summary>
            /// Configures Swagger for a specific named instance.
            /// </summary>
            /// <param name="name">The name of the Swagger configuration instance.</param>
            /// <param name="options">The SwaggerGen options to configure.</param>
            public void Configure(string name, SwaggerGenOptions options)
            {
                Configure(options);
            }
        }
}