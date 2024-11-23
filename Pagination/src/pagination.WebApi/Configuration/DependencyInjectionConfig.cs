using System;
using Pagination.Domain;
using Pagination.Infrastructure.Repositories;

namespace pagination.WebApi.Configuration;

public static class DependencyInjectionConfig
{
    public static void ResolveDependencies(this IServiceCollection services)
    {
       services.AddScoped<IProductRepository, ProductRepository>();
       services.AddScoped<IOrderRepository, OrderRepository>();

       services.AddScoped<IProductService, ProductService>();
       services.AddScoped<IOrderService, OrderService>();
    }
}
