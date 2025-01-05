using CompanyApi;
using CompanyApi.Entities;
using CompanyApi.Interceptors;
using CompanyApi.Models;
using CompanyApi.Options;
using CompanyApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.ConfigureOptions<DatabaseOptionSetup>();

builder.Services.AddHttpContextAccessor();

// builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Register interceptors
builder.Services.AddSingleton<QueryLoggingInterceptor>();
//builder.Services.AddSingleton<AuditTrialInterceptor>();
builder.Services.AddSingleton<TransactionInterceptor>();
builder.Services.AddSingleton<ConnectionInterceptor>();
builder.Services.AddSingleton<ErrorHandlingInterceptor>();

builder.Services.AddDbContext<AppDbContext>(
    (serviceProvider,dbContextoptionsBuilder) =>
    {
        var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

        
        dbContextoptionsBuilder.UseSqlServer(databaseOptions.ConnectionString, sqlServerAction =>
        {
            sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);

            sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout);
        });

        dbContextoptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);

        dbContextoptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);

        dbContextoptionsBuilder
                .AddInterceptors(serviceProvider.GetRequiredService<QueryLoggingInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<TransactionInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<ConnectionInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<ErrorHandlingInterceptor>());
                


    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}

app.UseHttpsRedirection();

app.MapGet("companies/{companyId:int}", async (int companyId, AppDbContext dbContext) =>
{
    var company = await dbContext
                    .Set<Company>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c=>c.Id == companyId);
    if (company is null)
        return Results.NotFound($"The company with Id '{companyId}' was not found.");

    var response = new CompanyResponse(company.Id, company.Name);
    return Results.Ok(response);
});

app.Run();


