using System.Reflection;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure.Persistence;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure;

public static class ServiceRegistration
{
    
    /// <summary>
    /// Configures and registers the persistence layer, including the database context and repositories.
    /// </summary>
    /// <param name="services">The service collection to add dependencies to.</param>
    /// <param name="configuration">The application configuration for retrieving connection strings.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure database context
          
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); // Use SQL Server in production
         

        // Register Unit of Work pattern
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register repositories dynamically
        services.RegisterRepositories();

        return services;
    }
    /// <summary>
    /// Registers all repository implementations dynamically based on their interface definitions.
    /// </summary>
    /// <param name="services">The service collection to add repositories to.</param>
    private static void RegisterRepositories(this IServiceCollection services)
    {
        var interfaceType = typeof(IGenericRepository<>);

        // Get all interfaces that inherit from IGenericRepository<>
        var interfaces = Assembly.GetAssembly(interfaceType)!.GetTypes()
            .Where(p => p.GetInterface(interfaceType.Name) != null);

        // Get all possible implementations of repositories
        var implementations = Assembly.GetAssembly(typeof(GenericRepository<>))!.GetTypes();

        // Loop through each repository interface and find its corresponding implementation
        foreach (var item in interfaces)
        {
            var implementation = implementations.FirstOrDefault(p => p.GetInterface(item.Name) != null);

            // Register the repository in the DI container if an implementation is found
            if (implementation is not null)
                services.AddTransient(item, implementation);
        }

    }

}