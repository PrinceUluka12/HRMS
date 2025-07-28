using Azure.Identity;
using HRMS.API.Extensions;
using HRMS.Application;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add application services
builder.Services.AddApplicationLayer();

// Add database context
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
    
// Add Azure AD authentication
builder.Services.AddAzureAdAuthentication(builder.Configuration);

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





var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}
app.UseConfiguredCors(); // Applies configured CORS policies
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerWithVersioning();
app.MapControllers();

app.Run();