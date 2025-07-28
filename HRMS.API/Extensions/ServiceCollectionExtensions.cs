using HRMS.Application.Common.Behaviors;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Mapping;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Positions.Queries.GetAllPositions;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Interfaces;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Persistence;
using HRMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Identity.Web;


namespace HRMS.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureAdAuthentication(this IServiceCollection services,
        IConfiguration configuration)   
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Bind JwtSettings from configuration
                configuration.Bind("JwtSettings", options);

                // Set authority and audience for Azure AD authentication
                options.Authority = configuration["JwtSettings:Authority"];
                options.Audience = configuration["JwtSettings:Audience"];

                // Configure token validation parameters (valid issuers)
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuers = configuration.GetSection("JwtSettings:ValidIssuers").Get<string[]>()
                };
            });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
                policy.RequireRole("Admin"));
            options.AddPolicy("Department.Manager", policy =>
                policy.RequireRole("Department.Manager"));
            options.AddPolicy("Payroll.Specialist", policy =>
                policy.RequireRole("Payroll.Specialist"));
            options.AddPolicy("Employee", policy =>
                policy.RequireRole("Employee"));
        });

        return services;
    }
}