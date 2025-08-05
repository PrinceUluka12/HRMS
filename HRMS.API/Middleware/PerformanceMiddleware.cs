using System.Diagnostics;

namespace HRMS.API.Middleware;

public class PerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceMiddleware> _logger;
    private readonly IConfiguration _configuration;
    private readonly int _warningThresholdMs;
    private readonly bool _logRequestQuery;

    public PerformanceMiddleware(
        RequestDelegate next,
        ILogger<PerformanceMiddleware> logger,
        IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _configuration = configuration;

        _warningThresholdMs = _configuration.GetValue<int>("Performance:WarningThresholdMs", 500);
        _logRequestQuery = _configuration.GetValue<bool>("Performance:LogRequestQuery", false);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;

            var logMessage = new
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                StatusCode = context.Response.StatusCode,
                DurationMs = elapsedMs,
                Query = _logRequestQuery ? context.Request.QueryString.Value : null
            };

            if (elapsedMs > _warningThresholdMs)
            {
                _logger.LogWarning("Slow Request {@LogMessage}", logMessage);
            }
            else
            {
                _logger.LogInformation("Request Completed {@LogMessage}", logMessage);
            }
        }
    }
}