using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Interfaces.Services;

public class PayrollProcessingService(
    IServiceProvider services,
    ILogger<PayrollProcessingService> logger)
    : BackgroundService
{
    private readonly TimeSpan _processingInterval = TimeSpan.FromDays(14);
    private readonly TimeSpan _targetStartTime = TimeSpan.FromHours(0); // 12:00 AM

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Payroll Processing Service is starting.");

        // Wait until the next scheduled start time (e.g., midnight)
        var now = DateTime.UtcNow;
        var todayMidnightUtc = now.Date + _targetStartTime;
        var nextRunTimeUtc = todayMidnightUtc > now ? todayMidnightUtc : todayMidnightUtc.AddDays(1);
        var initialDelay = nextRunTimeUtc - now;

        logger.LogInformation("Initial delay until first payroll run: {Delay}", initialDelay);

        await Task.Delay(initialDelay, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = services.CreateScope();
                var payrollService = scope.ServiceProvider.GetRequiredService<IPayrollService>();

                logger.LogInformation("Running scheduled payroll at {Time}", DateTime.UtcNow);
                await payrollService.ProcessScheduledPayrollsAsync(stoppingToken);
                logger.LogInformation("Payroll processing completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing payrolls.");
            }

            // Wait for the next scheduled run (14 days later)
            await Task.Delay(_processingInterval, stoppingToken);
        }

        logger.LogInformation("Payroll Processing Service is stopping.");
    }
}

public interface IPayrollService
{
    Task ProcessScheduledPayrollsAsync(CancellationToken cancellationToken);
}