using AutoSphere.Api.Data;
using AutoSphere.Api.Repositories;
using AutoSphere.Api.Services;
using Microsoft.EntityFrameworkCore;
using OpenSearch.Net;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog configuration
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Logs to the console
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // Logs to a file with daily rolling
    .Enrich.FromLogContext() // Adds context properties (e.g., request ID, user info)
    .Enrich.WithProperty("ApplicationName", "AutoSphere.API") // Adds a custom property
    .ReadFrom.Configuration(builder.Configuration) // Optional: Reads configuration from appsettings.json
    .CreateLogger();

// Add services to the container.
builder.Services.AddMemoryCache(); // Add memory cache service

// Replace default logging with Serilog
builder.Host.UseSerilog();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add OpenSearch client
var openSearchEndpoint = builder.Configuration["OpenSearch:Endpoint"];
var openSearchSettings = new ConnectionConfiguration(new Uri(openSearchEndpoint))
    .BasicAuthentication(
        builder.Configuration["OpenSearch:Username"],
        builder.Configuration["OpenSearch:Password"]
    );
var openSearchClient = new OpenSearchLowLevelClient(openSearchSettings);
builder.Services.AddSingleton<OpenSearchLowLevelClient>(openSearchClient);

//Add Services and Repositories
builder.Services.AddScoped<IVehicleSearchRepository, VehicleSearchRepository>();
builder.Services.AddScoped<IVehicleSearchService, VehicleSearchService>();
builder.Services.AddScoped<ISavedSearchRepository, SavedSearchRepository>();
builder.Services.AddScoped<ISavedSearchService, SavedSearchService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();


builder.Services.AddControllers();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Add custom exception-handling middleware
app.UseMiddleware<AutoSphere.Api.Middleware.ExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoSphere API v1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI the default page
    });
    app.MapOpenApi();
}

app.MapControllers();
app.UseHttpsRedirection();


app.Run();

