using Exception_ProblemDetails;
using Exception_ProblemDetails.Handlers;
using Exception_ProblemDetails.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Npgsql;

using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddProblemDetails();
// In case of multiple Exception handler and want to handle the Extensions use the delegate
builder.Services.AddProblemDetails(options => {
    options.CustomizeProblemDetails = (context) =>
    {
        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions = new Dictionary<string, object?>()
            {
                {"requestId", context.HttpContext.TraceIdentifier},
                {"traceId", activity?.Id},
                {"spanId", activity?.SpanId.ToString()}
            };
    };
});

builder.Services.AddExceptionHandler<GlobalExeptionHandler>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IToDoService, ToDoService>();


// OpenTelemetry information
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});


builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("ToDoApi"))
    .WithTracing(tracing => 
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddNpgsql()
            .AddOtlpExporter( opt => {
                opt.Endpoint = new Uri("http://localhost:4317");
                opt.Protocol = OtlpExportProtocol.Grpc;
            })
            .AddConsoleExporter(opt => {
                opt.Targets = ConsoleExporterOutputTargets.Console;           
            });     
    });

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseExceptionHandler();
//app.UseExceptionHandler(_ => { });
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
