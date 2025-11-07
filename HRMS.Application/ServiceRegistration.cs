using FluentValidation;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Mapping;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Application.SignalR;
using HRMS.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System.Reflection;

namespace HRMS.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Singleton services
        services.AddSingleton<IDateTime, DateTimeService>();
        

        // Scoped services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAzureAdService, AzureAdService>();
        services.AddScoped<IBenefitsService, BenefitsService>();
        services.AddScoped<ITimeTrackingService, TimeTrackingService>();
        services.AddScoped<ILeavePolicyService, LeavePolicyService>();
        services.AddScoped<IPayrollService, PayrollService>();
        services.AddScoped<ITaxRuleProvider, TaxRuleProvider>();
        services.AddScoped<ITeamsIntegrationService, TeamsIntegrationService>();
        services.AddScoped<IDefaultStageFactory, DefaultStageFactory>();
        services.AddScoped<IOnboardingService, OnboardingService>();
        services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
        services.AddScoped<INotificationPublisher, NotificationPublisher>();



        // Background/hosted services
        services.AddHostedService<PayrollProcessingService>();

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));

        return services;
    }
}