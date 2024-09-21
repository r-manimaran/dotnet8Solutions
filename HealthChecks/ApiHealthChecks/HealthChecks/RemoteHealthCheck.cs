﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Client;

namespace ApiHealthChecks.HealthChecks
{
    public class RemoteHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RemoteHealthCheck(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using(var httpClient = _httpClientFactory.CreateClient())
            {
                var response = await httpClient.GetAsync("https://api.ipify.org");
                if(response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy($"Remote api is healthy");
                }
                else
                {
                    return HealthCheckResult.Unhealthy("Remote api is not healthy");
                }
            }
        }
    }
}
