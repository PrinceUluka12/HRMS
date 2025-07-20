using HRMS.API.Extensions;
using HRMS.Application;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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

// Add authorization policies
builder.Services.AddAuthorizationPolicies();

builder.Services.AddHttpContextAccessor(); // Provides access to HTTP context

// Add Swagger
builder.Services.AddSwaggerWithAzureAdSupport(builder.Configuration);






var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();