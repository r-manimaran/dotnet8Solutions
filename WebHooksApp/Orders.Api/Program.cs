using Microsoft.EntityFrameworkCore;
using Orders.Api.Data;
using Orders.Api.Endpoints;
using Orders.Api.Extensions;
using Orders.Api.Repositories;
using Orders.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services.AddOpenApi();
builder.Services.AddSingleton<InMemoryOrderRepository>();
builder.Services.AddSingleton<InMemorySubscriptionRepository>();
builder.Services.AddHttpClient<WebhookDispatcher>();

// Added for Persistance data
builder.Services.AddHttpClient();
builder.Services.AddScoped<WebhookDispatcherUsingDB>();

builder.Services.AddDbContext<WebhookDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("webhooks")));

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });

    // Apply the Migration
    await app.ApplyMigrationAsync();

}

app.UseHttpsRedirection();

// Endpoints using InMemory
// app.MapOrderEndpoints();

// Endpoints using Postgres persistance
app.MapPersistentOrderEndpoints();

// Endpoints using InMemory
// app.MapWebhookEndpoints();

// Endpoint using Postgres persistance
app.MapPersistantWebhookEndpoints();

app.Run();


