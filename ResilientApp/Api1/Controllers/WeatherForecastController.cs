using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Hedging;
using Polly.Retry;
using Polly.Timeout;
using System.Net;

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
        /// <summary>
        /// Using Polly nuget packages
        /// </summary>
        /// <returns></returns>
        [HttpGet("Polly/retry")]
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
                var result = await retryPolicy.ExecuteAsync(ConnectToAPI);

                // using retry forever policy
                // var result = await retryForeverPolicy.ExecuteAsync(ConnectToAPI);

                // using retry with count and wait in between retries
                // var result = await waitRetryPolicy.ExecuteAsync(ConnectToAPI);

                // using retry forever with some intermediate wait between retries
                //var result = await waitRetryForeverPolicy.ExecuteAsync(ConnectToAPI);

                if (result.IsSuccessStatusCode)
                    return Ok(await result.Content.ReadFromJsonAsync<WeatherForecast[]>());

                throw new Exception();
            }
            catch (Exception ex)
            {
                return BadRequest("Sorry! Error occured");
            }

        }


        //Using Polly.Core packages
        [HttpGet("pollycore/retry")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetForecast()
        {

            #region Retry Strategy
            var retryStrategies = new RetryStrategyOptions
            {
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true, // Add a random factor to the delay
                MaxRetryAttempts = 4,
                Delay = TimeSpan.FromSeconds(5),
                OnRetry = static args =>
                {
                    Console.WriteLine("OnRetry, Attempt:{0}", args.AttemptNumber);
                    return default;
                },
                ShouldHandle = new PredicateBuilder().Handle<Exception>()
            };
            #endregion

            ResiliencePipeline retryPipeline = new ResiliencePipelineBuilder()
                .AddRetry(retryStrategies)
                .Build();

            var result = await retryPipeline.ExecuteAsync(async token =>
            {
                var response = await ConnectToAPI();
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<WeatherForecast[]>();
                else
                    throw new Exception(response.RequestMessage!.ToString());
            });
            return Ok(result);

        }
        /// <summary>
        /// Timeout Strategy
        /// </summary>
        /// <returns></returns>
        [HttpGet("pollycore/timeout")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> TimeOut()
        {
            var timeoutStratergy = new TimeoutStrategyOptions()
            {
                Timeout = TimeSpan.FromSeconds(5),
                OnTimeout = static args =>
                {
                    Console.WriteLine($"{args.Context.OperationKey}:Exception timed out after {args.Timeout.TotalSeconds} seconds");
                    return default;
                }
            };
            var timeoutPipeline = new ResiliencePipelineBuilder()
                                    .AddTimeout(timeoutStratergy)
                                    .Build();
            var result = await timeoutPipeline.ExecuteAsync(async token => await ConnectToApiWithCancellation(token));
            return Ok(result);
                
        }


        [HttpGet("pollycore/fallback")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Fallback()
        {
            var fallbackStrategy = new FallbackStrategyOptions<List<WeatherForecast>>()
            {
                ShouldHandle = new PredicateBuilder<List<WeatherForecast>>()
                    .Handle<Exception>()
                    .HandleResult(r => r is null),
                FallbackAction = static args =>
                {
                    var defaultWeather = new List<WeatherForecast>();
                    defaultWeather.Add( new WeatherForecast()
                    {
                        Date = DateOnly.FromDateTime(DateTime.UtcNow),
                        Summary ="Windy",
                        TemperatureC =34
                    });
                    return Outcome.FromResultAsValueTask(defaultWeather);
                },
                OnFallback = static args =>
                {
                    Console.WriteLine("Fallback returned");
                    return default;
                }
            };
            var fallbackPipeline = new ResiliencePipelineBuilder<List<WeatherForecast>>()
                    .AddFallback(fallbackStrategy)
                    .Build();
            var data = await fallbackPipeline.ExecuteAsync(async token=> await ConnectToApiWithCancellation(token));
            return Ok(data);
                    
        }

        [HttpGet("pollycore/circuitbreaker")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> CircuitBreaker()
        {
            var circuitBreakerStrategy = new CircuitBreakerStrategyOptions()
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(5),
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                OnOpened = args =>
                {
                    Console.WriteLine("Circuit is opened for {0} seconds", args.BreakDuration);
                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    Console.WriteLine("Circuit is closed, New call can be made");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    Console.WriteLine("Circuit is hal-open, next call is a trial.");
                    return ValueTask.CompletedTask;
                }  
            };

            var retryStrategies = new RetryStrategyOptions
            {
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true, // Add a random factor to the delay
                MaxRetryAttempts = 8,
                Delay = TimeSpan.FromSeconds(5),
                OnRetry = static args =>
                {
                    Console.WriteLine("OnRetry, Attempt:{0}", args.AttemptNumber);
                    return default;
                },
                ShouldHandle = new PredicateBuilder().Handle<Exception>()
            };

            var pipeline = new ResiliencePipelineBuilder()
                    .AddRetry(retryStrategies)
                    .AddCircuitBreaker(circuitBreakerStrategy)
                    .Build();
            var result = await pipeline.ExecuteAsync(async token => await ConnectToApiWithCancellation(token));
            return Ok(result);

        }

        [HttpGet("pollycore/hedging")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Hedging()
        {
            var hedgingStrategy = new HedgingStrategyOptions<HttpResponseMessage>()
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError),
                MaxHedgedAttempts = 3,
                Delay = TimeSpan.FromSeconds(5),
                ActionGenerator = static args =>
                {
                    Console.WriteLine("Preparing to execute hedged action.");
                    return () => args.Callback(args.ActionContext);
                },
                OnHedging = static args =>
                {
                    Console.WriteLine($"On Hedging: Attempt number {args.AttemptNumber}");
                    return default;
                }
            };

            var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                         .AddHedging(hedgingStrategy)
                         .Build();
            var result = await pipeline.ExecuteAsync(async token => await ConnectToAPI());
            if (result.IsSuccessStatusCode)
                return Ok(await result.Content.ReadFromJsonAsync<List<WeatherForecast>>());
            else
                return StatusCode((int)result.StatusCode);

        }

        [HttpGet("rate-limiting")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> RateLimiting()
        {
            var result = await ConnectToAPI();

            if (result.IsSuccessStatusCode)
                return Ok(await result.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>());
            else 
                return StatusCode((int)result.StatusCode);
        }

        [HttpGet("caching")]
        public async Task<IActionResult> Cache()
        {
           // await Task.Delay(3000);
            var data = await _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("https://localhost:7296/WeatherForecast");
            return Ok(data);
        }

        [HttpGet("clear-cache")]
        public async Task<IActionResult> ClearCache()
        {
            var data = await _httpClient.GetAsync("https://localhost:7296/WeatherForecast/clear-cache");

            return NoContent();
        }

        [HttpGet("caching-redis")]
        public async Task<IActionResult> CacheRedis()
        {
            //
            // await Task.Delay(3000);
            var data = await _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("https://localhost:7296/WeatherForecast/with-rediscache");
            return Ok(data);
        }

        [HttpGet("clear-redis-cache")]
        public async Task<IActionResult> ClearRedisCache()
        {
            
            var data = await _httpClient.GetAsync("https://localhost:7296/WeatherForecast/clear-redis-cache");

            return NoContent();
        }


        // Helper Methods
        private async Task<HttpResponseMessage> ConnectToAPI()
        {
            var response = await _httpClient.GetAsync("https://localhost:7296/WeatherForecast");
            return response;
        }


        // Timeout using the cancellationToken
        private async Task<List<WeatherForecast>> ConnectToApiWithCancellation(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("https://localhost:7296/WeatherForecast", cancellationToken);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<WeatherForecast>>(cancellationToken);
            else
                return default!;
        }
    }
}
