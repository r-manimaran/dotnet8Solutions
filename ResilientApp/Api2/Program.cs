using Microsoft.AspNetCore.RateLimiting;
using NetcodeHub.Packages.Wrappers.OutputCache;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRateLimiter(options =>
{
    options.AddTokenBucketLimiter("BucketTokenLimiter", opt =>
    {
        opt.AutoReplenishment = true;
        opt.TokenLimit = 3;
        opt.TokensPerPeriod = 3;
        opt.QueueLimit = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(5);

    });   

    //Set the rejection status code and handle rejected requests
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, cancellationToke) =>
    {
        await Task.CompletedTask;
    };
});

// builder.Services.AddOutputCache();
// Instead of above OutputCache used a anotherpackage which contains the option for Cache invalidation
builder.Services.AddNetcodeHubOutputCache();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379,password=eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81";
    options.InstanceName = "redis-cache";
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Use the RateLimiter
app.UseRateLimiter();

// use the output cache
//app.UseOutputCache();
app.UseNetcodeHubOutputCache();

app.MapControllers();

app.Run();
