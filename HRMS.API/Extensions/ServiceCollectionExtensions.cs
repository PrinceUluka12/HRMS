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
    /*public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(CreateEmployeeCommand).Assembly,
                typeof(EmployeeRepository).Assembly,
                typeof(GetAllPositionsQuery).Assembly);
            
            // Add behaviors in order of execution
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });
        
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        // Register repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPayrollRepository, PayrollRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        
        
        
        // Register services
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
        services.AddScoped<ITaxCalculationService, TaxCalculationService>();
        services.AddScoped<ITaxRuleProvider, TaxRuleProvider>();
        services.AddScoped<ITeamsIntegrationService, TeamsIntegrationService>();
        
        // Register current user service
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        
        
        return services;
    }*/

    /*public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
    */
    
    public static IServiceCollection AddAzureAdAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddMicrosoftGraph(configuration.GetSection("MicrosoftGraph"))
                .AddInMemoryTokenCaches();
        
        
        services.AddMicrosoftGraph(opt =>
        {
            configuration.Bind("AzureAd", opt); 
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