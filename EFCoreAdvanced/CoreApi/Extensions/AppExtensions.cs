using CoreApi.Data;
using CoreApi.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Net;
using System.Text.Json;
namespace CoreApi.Extensions;

public static class AppExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDbContextPool<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"))
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
}
