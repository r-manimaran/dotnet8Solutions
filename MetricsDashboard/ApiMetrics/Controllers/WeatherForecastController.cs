using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace ApiMetrics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMeterFactory _meterFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IMeterFactory meterFactory)
        {
            _logger = logger;
            _meterFactory = meterFactory;
        }

        [HttpGet("weather/{city}")]
        public async Task<IActionResult> Get([FromRoute] string city, [FromQuery]int days = 5)
        {
            _logger.LogInformation("Request received for {City}", city);
            if(Random.Shared.Next(1,1000) == 10)
            {
                throw new Exception("Something went wrong");
            }

            var forecasts = Enumerable.Range(1, days).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            var meter = _meterFactory.Create("WeatherForecastApi");
            var counter = meter.CreateCounter<int>("city_counter");
            counter.Add(1, new KeyValuePair<string, object?>("city", city));
            meter.Dispose();

            _logger.LogInformation("Returning {Count} forecasts for {City}", forecasts.Length, city);
            await Task.Delay(Random.Shared.Next(5, 100));

            return Ok(new WeatherForecastResponse 
            { 
                City = city,
                Forecasts = forecasts 
            });
        }
    }
}
