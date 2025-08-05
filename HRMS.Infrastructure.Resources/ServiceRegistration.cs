using HRMS.Application.Interfaces;
using HRMS.Infrastructure.Resources.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure.Resources;

public static class ServiceRegistration
{
    public static IServiceCollection AddResourcesInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ITranslator, Translator>();

        return services;
    }
}