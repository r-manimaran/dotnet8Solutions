using Microsoft.Extensions.Caching.Hybrid;
using System.Net;
using ZiggyCreatures.Caching.Fusion;

namespace HybridCacheApi;

public class WeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly HybridCache _cache;

    // Can Also use IFusionCache instead of HybridCache
    private readonly IFusionCache _fusionCache;
    public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration, HybridCache cache, IFusionCache fusionCache)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _cache = cache;
        _fusionCache = fusionCache;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string cityCountry)
    {
        var cacheKey = $"weather-{cityCountry}";
        
        return await _cache.GetOrCreateAsync<WeatherResponse?>(cacheKey, async entry =>
                  await GetWeatherAsync(cityCountry), tags: ["weather"]);
        // If you want to invalidate or clear the cache for all the weather entries you can use the tags to remove them all
        
    }
    public async Task<WeatherResponse?> GetWeatherAsync(string city)
    {
        string apiKey = _configuration["OpenWeatherApiKey"]!;

        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";
        
        var httpClient = _httpClientFactory.CreateClient();
        
        var response = await httpClient.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        return await response.Content.ReadFromJsonAsync<WeatherResponse>();
    }
}
