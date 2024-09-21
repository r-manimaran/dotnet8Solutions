using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace ApiHealthChecks.HealthChecks
{
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<MemoryHealthCheckOptions> _options;
        public string Name => "MemoryHealthCheck";
        public MemoryHealthCheck(IOptionsMonitor<MemoryHealthCheckOptions> options)
        {
            _options = options;
        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var options = _options.Get(context.Registration.Name);
            var allocatedMemory = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>
            {
                { "AllocatedMemory", allocatedMemory },
                {"Gen0Collection",GC.CollectionCount(0)},
                {"Gen1Collection",GC.CollectionCount(1)},
                {"Gen2Collection",GC.CollectionCount(2)},
            };

            var status = (allocatedMemory < options.Threshold) ? HealthStatus.Healthy : HealthStatus.Unhealthy;
            return Task.FromResult(new HealthCheckResult(status,
                description: options.MemoryStatus,
                exception:null, data));
        }
    }

    public class MemoryHealthCheckOptions
    {
        public string MemoryStatus { get; set; }
        public long Threshold { get; set; } = 1024L * 1024L * 1024L; // 1 GB
    }
}
