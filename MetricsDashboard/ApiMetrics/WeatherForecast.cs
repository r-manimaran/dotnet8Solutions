namespace ApiMetrics
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public class WeatherForecastResponse
    {
        public string City { get; set; } = string.Empty;    
        public WeatherForecast[] Forecasts { get; set; } = Array.Empty<WeatherForecast>();
    }
}
