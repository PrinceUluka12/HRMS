using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using FluentValidation.AspNetCore;
using HRMS.API.Extensions;
using HRMS.API.Filters;
using HRMS.API.Middleware;
using HRMS.Application;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Hubs;
using HRMS.Infrastructure;
using HRMS.Infrastructure.Persistence;
using HRMS.Infrastructure.Resources;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Graph;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for structured logging. This reads configuration from appsettings (if provided)
// and enriches logs with contextual information. It writes logs to both the console and a file sink.
builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        // Write logs to the console for local debugging
        .WriteTo.Console()
        // Write logs to a rolling file to allow centralized collection (e.g., shipping to ELK or Application Insights)
        .WriteTo.File("Logs/hrms-.log", rollingInterval: RollingInterval.Day);
});

// Add services to the container and register the ApiExceptionFilter globally
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});

// Automatically register FluentValidation validators and enable automatic model validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();

// Add services to the container
builder.Services.AddControllers();

// Add application services
builder.Services.AddApplicationLayer(builder.Configuration);

// Add database context
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

builder.Services.AddResourcesInfrastructure();
    
// Add Azure AD authentication
builder.Services.AddAzureAdAuthentication(builder.Configuration);

// SignalR
builder.Services.AddSignalR();

// Your Key Vault URI
string keyVaultUrl = "https://hrmscred.vault.azure.net/";

// Create a secret client with DefaultAzureCredential
var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

// Fetch the secrets

KeyVaultSecret clientIdSecret = await client.GetSecretAsync("HrmsClientId");
KeyVaultSecret tenantIdSecret = await client.GetSecretAsync("HrmsTenantId");
KeyVaultSecret clientSecret = await client.GetSecretAsync("HrmsClientSecret");

string clientId = clientIdSecret.Value;
string tenantId = tenantIdSecret.Value;
string clientSecretString =  clientSecret.Value;


builder.Services.AddSingleton<GraphServiceClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    
    var tenantId = tenantIdSecret.Value;
    var clientId = clientIdSecret.Value;
    var clientSecretString = clientSecret.Value;

    var scopes = new[] { "https://graph.microsoft.com/.default" };

    var credential = new ClientSecretCredential(tenantId, clientId, clientSecretString);
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

// -----------------------------------------------------------------------------
// Health checks and metrics
//
// Register health checks for the database context and optionally Key Vault or
// external services. Additional checks can be added using the fluent API.
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("Database");

// Allow large uploads (adjust for your needs)
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 1024L * 1024L * 200L; // 200 MB
});
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwaggerWithVersioning();

// Needed by PdfSharpCore for some encodings
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Global Middleware
app.UseMiddleware<PerformanceMiddleware>();  // Optional if using
app.UseMiddleware<ExceptionHandlingMiddleware>(); // Logs errors
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseConfiguredCors(); // after auth to avoid CORS issues
app.UseCustomLocalization(); // if culture affects routes or headers
// Log HTTP requests for diagnostics
app.UseSerilogRequestLogging();
app.MapGet("/api/health", () => Results.Ok("HRMS API is up and running."));
app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();
// Map health check endpoint for monitoring tools (e.g., Prometheus, Application Insights)
app.MapHealthChecks("/healthz");
app.Run();

// Expose Program class for integration testing
public partial class Program { }