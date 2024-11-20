var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("weather", client =>
{
    client.BaseAddress = new Uri("http://localhost:5135");
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/weatherforecast", async(IHttpClientFactory factory) =>
{
   using HttpClient client = factory.CreateClient("weather");

   var forecast = await client.GetFromJsonAsync<WeatherForecast[]>("weatherforecast");
   
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
