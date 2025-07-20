using System.Reflection;
using FluentValidation;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Mapping;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Interfaces.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;

namespace HRMS.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Register current user service
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        // Registers  services implementation.
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAzureAdService, AzureAdService>();
        services.AddScoped<IBenefitsService, BenefitsService>();
        services.AddSingleton<IDateTime, DateTimeService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<GraphServiceClient>(sp =>
        {
            var factory = sp.GetRequiredService<GraphServiceClientFactory>();
            return factory.CreateClient();
        });
        services.AddScoped<ILeavePolicyService, LeavePolicyService>();
        services.AddScoped<IPayrollService, PayrollService>();
        services.AddHostedService<PayrollProcessingService>();
        services.AddScoped<ITaxRuleProvider, TaxRuleProvider>();
        services.AddScoped<ITeamsIntegrationService, TeamsIntegrationService>();

        // Registers all FluentValidation validators from the executing assembly.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Registers MediatR and automatically discovers handlers from the current assembly.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
            
            
        return services;
    }
}