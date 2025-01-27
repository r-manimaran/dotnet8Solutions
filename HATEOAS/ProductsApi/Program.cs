using ProductsApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddServices();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddSwagger();

app.UseHttpsRedirection();

app.ApplyMigration();

app.AddApiEndpoints();

app.Run();
