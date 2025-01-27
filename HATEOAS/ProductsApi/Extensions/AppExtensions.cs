using Bogus;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Endpoints;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.Extensions;

public static class AppExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("ProductsDb");

            options.UseAsyncSeeding(async (context, _, token) =>
            {
                var productsFaker = new Faker<Product>()
                    .RuleFor(p => p.Id, f => f.Random.Guid())
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Sku, f => f.Commerce.Ean13())
                    .RuleFor(p => p.Currency, f => f.Finance.Currency().Code)
                    .RuleFor(p => p.Amount, f => f.Finance.Amount(1, 1000, 2));
                var productsToSeed = productsFaker.Generate(50);

                if (!await context.Set<Product>().AnyAsync())
                {
                    await context.Set<Product>().AddRangeAsync(productsToSeed, token);
                    await context.SaveChangesAsync(token);
                }
            });
        });
    }
    public static void AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IProductService, ProductService>();    
        services.AddScoped<ILinkService, LinkService>();
    }

    public static void AddApiEndpoints(this WebApplication app)
    {
        app.MapProductsEndpoints();
    }

    public static void AddSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    public static async void ApplyMigration(this IApplicationBuilder app)
    {
       using(var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}
