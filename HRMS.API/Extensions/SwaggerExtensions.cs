using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HRMS.API.Extensions;

public static class SwaggerExtensions
{
    /// <summary>
    /// Configures Swagger UI to display API documentation.
    /// </summary>
    public static IApplicationBuilder UseSwaggerWithVersioning(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "HRMS API v1");
            options.DocumentTitle = "HRMS API Docs";
        });

        return app;
    }

    /// <summary>
    /// Adds Swagger documentation services without API versioning.
    /// </summary>
    public static IServiceCollection AddSwaggerWithVersioning(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
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
                    },
                    new List<string>()
                }
            });

            setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = Assembly.GetExecutingAssembly().GetName().Name,
                Version = "v1",
                Description = "HRMS API"
            });

            setup.CustomSchemaIds(type =>
            {
                if (!type.IsGenericType) return type.Name;

                var genericTypeName = type.GetGenericTypeDefinition().Name;
                genericTypeName = genericTypeName[..genericTypeName.IndexOf('`')]; // Remove `1, `2 etc.

                var genericArgs = string.Join("_", type.GenericTypeArguments
                    .Select(t => t.Name.Replace("[]", "Array")));

                return $"{genericTypeName}Of{genericArgs}";
            });
        });

        return services;
    }
}
