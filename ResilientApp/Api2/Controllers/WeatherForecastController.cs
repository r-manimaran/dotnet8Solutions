using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using NetcodeHub.Packages.Wrappers.OutputCache;
using System.Text.Json;

namespace Api2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger,
                                INetcodeHubOutputCache netcodeHubOutputCache,
                                IDistributedCache distributedCache) : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger = logger;

        [HttpGet(Name = "GetWeatherForecast")]
        [EnableRateLimiting("BucketTokenLimiter")]
        //[OutputCache(Duration = 30, NoStore = false)]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            // Get From Cache
            var weatherData = await netcodeHubOutputCache.GetCacheAsync("weatherData", CancellationToken.None);
            if (!string.IsNullOrEmpty(weatherData))
            {
                return Ok(JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(weatherData));
            }

            // Get from DB and set the cache
            await Task.Delay(3000);
            var data =  Enumerable.Range(1, 15).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            await netcodeHubOutputCache.SetCacheAsync(cacheKey:"weatherData", 
                                             data:JsonSerializer.Serialize(data),
                                             ValidFor:TimeSpan.FromSeconds(30), 
                                             tags:null!, 
                                             cancellationToken:CancellationToken.None);
            return Ok(data);
        }

        [HttpGet("with-rediscache")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWithDistributedCache()
        {
            // Get From Cache
            var weatherData = await distributedCache.GetStringAsync("weatherData", CancellationToken.None);
            if (!string.IsNullOrEmpty(weatherData))
            {
                return Ok(JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(weatherData));
            }

            // Get from DB and set the cache
            await Task.Delay(3000);
            var data = Enumerable.Range(1, 15).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };

            await distributedCache.SetStringAsync("weatherData", JsonSerializer.Serialize(data),
                                            options, CancellationToken.None);
            return Ok(data);

        }

        [HttpGet("clear-cache")]
        public async Task<IActionResult> ClearCache ()
        {
            await netcodeHubOutputCache.RemoveCacheAsync("weatherData", CancellationToken.None);
            return NoContent();
        }

        [HttpGet("clear-redis-cache")]
        public async Task<IActionResult> ClearRedisCache()
        {
            await distributedCache.RemoveAsync("weatherData", CancellationToken.None);
            return NoContent();
        }



    }
}
