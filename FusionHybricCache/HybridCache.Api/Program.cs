using HybridCacheApi;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Scalar.AspNetCore;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<WeatherService>();

builder.Services.AddFusionCache()
    .WithDefaultEntryOptions(options => options.Duration = TimeSpan.FromMinutes(5))
    .WithSerializer(new FusionCacheSystemTextJsonSerializer())
    .WithDistributedCache(new RedisCache(new RedisCacheOptions
    {
        Configuration = "localhost:6379" // Running Redis in Docker
    })).AsHybridCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/weather/{city}", async (string city, WeatherService weatherService) =>
{
    var weather = await weatherService.GetCurrentWeatherAsync(city);
    return weather is null? Results.NotFound() : Results.Ok(weather);
});

app.MapGet("/clear/{tag}", async (string tag, HybridCache cache) =>
{
    await cache.RemoveByTagAsync(tag);
    return Results.Ok();
});


app.Run();

