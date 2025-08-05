using Azure.Identity;
using HRMS.API.Extensions;
using HRMS.API.Filters;
using HRMS.API.Middleware;
using HRMS.Application;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Hubs;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure;
using HRMS.Infrastructure.Persistence;
using HRMS.Infrastructure.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add application services
builder.Services.AddApplicationLayer();

// Add database context
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

builder.Services.AddResourcesInfrastructure();
    
// Add Azure AD authentication
builder.Services.AddAzureAdAuthentication(builder.Configuration);

// SignalR
builder.Services.AddSignalR();

builder.Services.AddSingleton<GraphServiceClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    
    var tenantId = config["AzureAd:TenantId"];
    var clientId = config["AzureAd:ClientId"];
    var clientSecret = config["AzureAd:ClientSecret"];

    var scopes = new[] { "https://graph.microsoft.com/.default" };

    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
    return new GraphServiceClient(credential, scopes);
});


// Add authorization policies
builder.Services.AddAuthorizationPolicies();

builder.Services.AddHttpContextAccessor(); // Provides access to HTTP context

// Add Swagger
builder.Services.AddSwaggerWithVersioning();

// Add Cors config 
builder.Services.AddConfiguredCors(builder.Configuration);

// Enables localization support
builder.Services.AddCustomLocalization(builder.Configuration); 

/*builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});*/
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwaggerWithVersioning();

// Global Middleware
app.UseMiddleware<PerformanceMiddleware>();  // Optional if using
app.UseMiddleware<ExceptionHandlingMiddleware>(); // Logs errors
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseConfiguredCors(); // after auth to avoid CORS issues
app.UseCustomLocalization(); // if culture affects routes or headers
app.MapGet("/api/health", () => Results.Ok("HRMS API is up and running."));
app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();
app.Run();