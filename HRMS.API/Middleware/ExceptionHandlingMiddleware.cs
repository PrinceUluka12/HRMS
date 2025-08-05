using FluentValidation;
using HRMS.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred");
            await HandleExceptionAsync(context, ex, context.Request.Path);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, string path)
    {
        context.Response.ContentType = "application/problem+json";

        switch (exception)
        {
            case ValidationException validationException:
                var validationProblem = new ValidationProblemDetails(
                    validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Failed",
                    Instance = path
                };

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(validationProblem);
                break;

            case NotFoundException:
                await WriteProblemDetailsAsync(context, StatusCodes.Status404NotFound, "Not Found", exception.Message, path);
                break;

            case ForbiddenAccessException:
                await WriteProblemDetailsAsync(context, StatusCodes.Status403Forbidden, "Forbidden", exception.Message, path);
                break;

            case ApplicationException:
                await WriteProblemDetailsAsync(context, StatusCodes.Status400BadRequest, "Application Error", exception.Message, path);
                break;

            default:
                await WriteProblemDetailsAsync(context, StatusCodes.Status500InternalServerError, "Server Error", exception.Message, path);
                break;
        }
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, int statusCode, string title, string detail, string instance)
    {
        context.Response.StatusCode = statusCode;
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = instance
        };
        return context.Response.WriteAsJsonAsync(problem);
    }
}
