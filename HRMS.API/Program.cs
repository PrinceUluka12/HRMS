using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HRMS.API.Extensions;
using HRMS.API.Middleware;
using HRMS.Application;
using HRMS.Application.Hubs;
using HRMS.Infrastructure;
using HRMS.Infrastructure.Resources;
using Microsoft.Graph;


var builder = WebApplication.CreateBuilder(args);

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