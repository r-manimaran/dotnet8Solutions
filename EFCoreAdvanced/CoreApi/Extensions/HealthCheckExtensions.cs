using CoreApi.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CoreApi.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthCheck(this  IServiceCollection services)
    {
        //services.AddHealthChecks()
        //    .AddDbContextCheck<AppDbContext>("Database",
        //            failureStatus: HealthStatus.Degraded,
        //            tags: new[] { "database" })
        //    .AddCheck("Custom", () =>
        //              HealthCheckResult.Healthy("Custom check is healthy"));
        return services;
    }

    public static WebApplication UseCustomHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions {
            ResponseWriter = async(context,report) => {
                context.Response.ContentType ="application/json";

                var response = new {
                    status = report.Status.ToString(),
                    Checks = report.Entries.Select(x=> new {
                        Component = x.Key,
                        Status = x.Value.Status.ToString(),
                        Description = x.Value.Description

                    }),
                    TotalDuration = report.TotalDuration
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        });
        return app;
    }
}

