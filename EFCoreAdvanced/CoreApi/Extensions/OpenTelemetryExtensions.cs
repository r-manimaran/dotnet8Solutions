using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Exporter.Prometheus;

namespace CoreApi.Extensions;

public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder ConfigureOpenTelemetry(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));

        var resourceBuilder = ResourceBuilder.CreateDefault()
                            .AddService(builder.Environment.ApplicationName)
                            .AddTelemetrySdk()
                            .AddAttributes(new Dictionary<string, object>
                            {
                                ["deployment.environment"] = builder.Environment.EnvironmentName
                            });

        builder.Services.AddOpenTelemetry()
                        .WithMetrics(metrics =>
                        {
                            metrics.AddPrometheusExporter() // Ensure you have the OpenTelemetry.Exporter.Prometheus package installed
                                .AddMeter("Microsoft.AspNetCore.Hosting")
                                .AddMeter("Microsoft.AspNetCore.Http")
                                .SetResourceBuilder(resourceBuilder);
                        })
                        .WithTracing(tracing =>
                        {
                            tracing.AddSource("APITracing")
                            .SetResourceBuilder(resourceBuilder)
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation() // This line requires the OpenTelemetry.Instrumentation.Http namespace
                            .AddOtlpExporter(o => // Ensure you have the OpenTelemetry.Exporter.OpenTelemetryProtocol package installed
                            {
                                o.Endpoint = new Uri(builder.Configuration["Jaeger:Endpoint"]
                                ?? throw new InvalidOperationException("Jaeger:Endpoint configuration is missing"));
                            });
                        });

        return builder;
    }
}
