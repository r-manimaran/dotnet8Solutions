using OpenTelemetry.Metrics;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);


    

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithMetrics(x =>
    {
        x.AddPrometheusExporter();
        x.AddMeter("Microsoft.AspNetCore.Hosting",
                      "Microsoft.AspNetCore.Server.Kestrel",
                      "WeatherForecastApi");

        x.AddView("request_duration_seconds",
                  new ExplicitBucketHistogramConfiguration
                  {
                      Boundaries = new double[] { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1 }
                  });
    });
builder.Services.AddMetrics();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapPrometheusScrapingEndpoint();

app.UseHttpsRedirection();
app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllers();

app.Run();
