using System.Diagnostics;

namespace HRMS.API.Middleware;

public class PerformanceMiddleware(
    RequestDelegate next,
    ILogger<PerformanceMiddleware> logger,
    IConfiguration configuration)
{
    private readonly int _warningThresholdMs = configuration.GetValue<int>("Performance:WarningThresholdMs", 500);

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            if (elapsedMs > _warningThresholdMs)
            {
                logger.LogWarning(
                    "Long running request: {Method} {Path} took {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path,
                    elapsedMs);
            }
            else
            {
                logger.LogInformation(
                    "Request completed: {Method} {Path} took {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path,
                    elapsedMs);
            }
        }
    }
}