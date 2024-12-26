using Validation.Api;
using Microsoft.Extensions.Options;
using FluentValidation;
using Carter;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);
// Register All FluentValidations in the assembly

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddCarter();

// Step 1: Validate IOptions using the DataAnnotation Validation.
/*builder.Services.AddOptions<GitHubSettings>()
    .BindConfiguration(GitHubSettings.ConfigurationSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();*/

// Step 2: Validate IOptions using the Fluent Validation
/*builder.Services.AddOptions<GitHubSettings>()
       .BindConfiguration(GitHubSettings.ConfigurationSection)
       .ValidateFluentValidation()
       .ValidateOnStart();*/

// Step 3: Replace Step 3 with a extensions method
builder.Services.AddOptionsWithFluentValidation<GitHubSettings>(GitHubSettings.ConfigurationSection);

builder.Services.AddHttpClient<GitHubService>((sp, httpClient) =>
{
    var gitHubSettings = sp.GetRequiredService<IOptions<GitHubSettings>>().Value;
    httpClient.DefaultRequestHeaders.Add("Authorization",gitHubSettings.AccessToken);
    httpClient.DefaultRequestHeaders.Add("User-Agent",gitHubSettings.UserAgent);

    httpClient.BaseAddress = new Uri(gitHubSettings.BaseAddress);
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapCarter();

try
{
    app.Run();
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}
