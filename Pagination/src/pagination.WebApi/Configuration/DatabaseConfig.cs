using System;
using Microsoft.EntityFrameworkCore;
using Pagination.Domain;
using Pagination.Infrastructure.Context;

namespace pagination.WebApi.Configuration;

public static class DatabaseConfig
{
    public static void ConfigureDatabase(
                this IServiceCollection services,
                IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void CreateDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();           
        }
    }

    public static void SeedDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var numberOfCategoriesToSeed = 50;
            var numberOfProductsToSeed = 100_000;
            var numberOfOrdersToSeed = 1_000;

            if(!context.Categories.Any())
            {
                for (int i = 1; i <= numberOfCategoriesToSeed; i++)
                {
                    var category = new Category
                    {
                        Name = $"Category {i}"
                    };
                    context.Categories.Add(category);                    
                }
                context.SaveChanges();
            }

            // Insert Product, select Random Category id
            if(!context.Products.Any())
            {
               var products = Enumerable.Range(1, numberOfProductsToSeed)
                    .Select(i => new Product
                    {
                        Name = $"Product {i}",
                        CategoryId  = Random.Shared.Next(1,50),
                        Price = Random.Shared.Next(1, 1000),
                        Stock = Random.Shared.Next(1, 1000),
                        CreatedAt = DateTime.Now.AddDays(-Random.Shared.Next(1, 365))                        
                    });

                context.Products.AddRange(products);
                   
                context.SaveChanges();
            }

            // Insert Order
            if(!context.Orders.Any())
            {
                var orders = Enumerable.Range(1, numberOfOrdersToSeed)
                    .Select(i => new Order
                    {
                       Code = $"Order {i}",                       
                    });

                context.Orders.AddRange(orders);

                context.SaveChanges();
            }
        }
    }
}
