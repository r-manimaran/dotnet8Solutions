using Carter;
using CoreApi.Data;
using CoreApi.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHealthChecks()
        .AddDbContextCheck<AppDbContext>("Database",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "database" })
        .AddCheck("Custom", () =>
                  HealthCheckResult.Healthy("Custom check is healthy"));

builder.ConfigureDatabase(builder.Configuration);

builder.Services.AddCarter();

builder.ConfigureLogging()
       .ConfigureOpenTelemetry();

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
        "/openapi/v1.json", "OpenAPI v1");
    });
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapCarter();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();

