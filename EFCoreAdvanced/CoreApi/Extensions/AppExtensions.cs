using CoreApi.Data;
using CoreApi.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Net;
using System.Text.Json;
using CoreApi.Models.Entities;
namespace CoreApi.Extensions;

public static class AppExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDbContextPool<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .UseLazyLoadingProxies()
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging(builder.Environment.IsDevelopment()); // For development only
        });
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void ConfigureFluentValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();
    }
   

    public static void MapMiddlewares(this IApplicationBuilder app)
    {

        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
             {
                 context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                 context.Response.ContentType = "application/problem+json";

                 await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(new 
                 {
                     Title = "An error occurred",
                     Detail = "An unexpected error occured",
                     Status = context.Response.StatusCode
                 }));
             });
        });
    }

    public static void PrepareDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();

        List<User> users =
        [
            new User
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = new Email("john.doe@gmail.com"),
                Address = new Address
                {
                    City = "New York",
                    State = "NY",
                    Street = "123 Main St",
                    ZipCode = "10001"
                },
                Metadata = new UserMetadata
                {
                    AccountCreated = DateTime.Now,
                    CreatedBy = "Admin",
                    Tags = new List<string> { "Admin", "User" }
                },
                Orders = new List<Order>
                {
                    new Order
                    {
                        OrderNumber= "1",
                        UserId = 1,
                        Total = 1000,
                        OrderDate = DateTime.Now
                    }
                }
            }
        ];

        context.AddRange(users);
        context.SaveChanges();
    }
}
