using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
namespace ApiHealthChecks.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration["ConnectionStrings:DefaultConnection"]!, healthQuery: "SELECT 1", name: "SQL Server", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Api", "Database" })
                //.AddRedis(configuration["Redis:ConnectionString"], name: "Redis")
                //.AddNpgSql(configuration["ConnectionStrings:DefaultConnection"], name: "PostgreSQL")
                //.AddRabbitMQ(configuration["RabbitMQ:ConnectionString"], name: "RabbitMQ")
                //.AddRabbitMQ(configuration["RabbitMQ:ConnectionString"], name: "RabbitMQ", tags: new[] { "rabbitmq" }))
                .AddCheck<RemoteHealthCheck>("Remote endpoint HealthCheck", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Api", "Remote" })
                .AddCheck<MemoryHealthCheck>("Custom HealthCheck", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Api", "Custom" })
                .AddCheck<RandomHealthCheck>("Random HealthCheck", tags: new[] { "Api", "Random" })
                .ForwardToPrometheus();

            return services;
        }

        public static IServiceCollection AddHealthChecksUI(this IServiceCollection services)
        {
            services.AddHealthChecksUI(setupSettings:setup =>
            {
                setup.SetEvaluationTimeInSeconds(60);//time in seconds between check
                setup.MaximumHistoryEntriesPerEndpoint(60); // max histories
                setup.SetApiMaxActiveRequests(1);
                setup.AddHealthCheckEndpoint("HealthCheck API", "/health");
            }).AddInMemoryStorage();

            return services;
        }

        public static IApplicationBuilder UseHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            
            app.UseHealthChecksUI(delegate(Options options)
            {
                options.UIPath = "/health-ui";             
            });

            return app;
        }

    }
}
