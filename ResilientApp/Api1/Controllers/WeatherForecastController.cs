using Microsoft.AspNetCore.Mvc;
using Polly;

namespace Api1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClient _httpClient; 
        public WeatherForecastController(ILogger<WeatherForecastController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            _logger.LogInformation("Using Retry Logic.");
            try
            {
                #region Retry policy to try 3 retries
                var retryPolicy = Policy.Handle<Exception>().RetryAsync(retryCount: 3, onRetry: (exception, retryCount) =>
                {
                    Console.WriteLine($"Error:{exception.Message}-Retry:{retryCount}");
                });
                #endregion

                #region RetryForever Policy
                var retryForeverPolicy = Policy.Handle<Exception>().RetryForeverAsync((exception, retryCount, context) =>
                {
                    Console.WriteLine($"Retry Attemp:{retryCount}, Error:{exception.Message}");
                });
                #endregion


                #region Wait and Retry specified number of times Policy
                TimeSpan waitingTime = TimeSpan.FromSeconds(10);
                var waitRetryPolicy = Policy.Handle<Exception>()
                                        .WaitAndRetryAsync(retryCount: 3,
                                                           sleepDurationProvider: retryAttempt => waitingTime,
                                                           onRetry: (exception, retryCount) =>
                                                           {
                                                               _logger.LogError($"Error:{exception.Message} - Retry:{retryCount}");
                                                           });

                #endregion

                #region Wait and Retry forever policy
                var waitRetryForeverPolicy = Policy.Handle<Exception>()
                                                   .WaitAndRetryForeverAsync(
                                                    sleepDurationProvider: time => waitingTime,
                                                    onRetry: (exception, retryCount) =>
                                                    {
                                                        _logger.LogError($"Retry Attempt:{retryCount},Error:{exception.Message}");
                                                    });
                #endregion
                // using the retry policy call the method which needs to be retried
                //var result = await retryPolicy.ExecuteAsync(ConnectToAPI);

                // using retry forever policy
                // var result = await retryForeverPolicy.ExecuteAsync(ConnectToAPI);

                // using retry with count and wait in between retries
                // var result = await waitRetryPolicy.ExecuteAsync(ConnectToAPI);

                // using retry forever with some intermediate wait between retries
                var result = await waitRetryForeverPolicy.ExecuteAsync(ConnectToAPI);

                if (result.IsSuccessStatusCode)
                    return Ok(await result.Content.ReadFromJsonAsync<WeatherForecast[]>());

                throw new Exception();
            }
            catch (Exception ex)
            {
                return BadRequest("Sorry! Error occured");
            }

        }

        private async Task<HttpResponseMessage> ConnectToAPI()
        {
            var response = await _httpClient.GetAsync("https://localhost:7296/WeatherForecast");
            return response;
        }

    }
}
