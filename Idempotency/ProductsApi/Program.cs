using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Endpoints;
using ProductsApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("products"));

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IIdemptencyService, IdempotencyService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => 
            options.SwaggerEndpoint("/openapi/v1.json", "Open API V1"));
}

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();
