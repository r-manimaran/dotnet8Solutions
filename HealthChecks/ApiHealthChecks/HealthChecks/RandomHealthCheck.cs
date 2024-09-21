using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiHealthChecks.HealthChecks
{
    public class RandomHealthCheck : IHealthCheck
    {
        private Random _random = new Random();
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var responseTime = _random.Next(1, 300);
            if (responseTime < 100)
            {
                return Task.FromResult(HealthCheckResult.Healthy("API is healthy"));
            }
            else if(responseTime < 200)
            {
                return Task.FromResult(HealthCheckResult.Degraded("API is degraded"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("API is unhealthy"));
        }
    }
}
