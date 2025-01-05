# Resiliency and Robust: Implement Retry on HTTP Requests using Polly

Polly:
 - .Net Library help to implement fault-tolerance and resilience strategies in the applications.
 - Provides rich set of features to define the policies to handle various types of transient faults, such as network interruptions, timeouts and resource unavailability.
 - This policies enable applications to recover gracefully from unexcepted errors, ensuring stability, reliability and an enhanced user experience.

**Polly Policies**:

1. Simple Retry With retry Count Policy:
     - Automatically retries a Http request call for a specified number of time(s), in cause of failure. 

2. Retry Forever Policy:
    - Continuously retry operation indefinitely when exception occur.

3. Wait and Retry Policy:
    - Retry n times with a delay wait between each retry.

4. Wait Retry Forever Policy:
    - Infinite retries with a waiting period between attempts.

**Using Polly.Core package Features**

1. Retry Strategy :
2. Timeout Strategy :
3. CircuitBreaker Strategy :
4. Fallback Strategy:
5. Hedging Strategy:


## Implement BucketTokenLimiter RateLimiter in API 2


## Output Cache
- The Output Cache middleware provides caching capabilities for the API responses, which can help improve performance by serving cached responses for subsequent identical requests.

- You can configure various caching policies based on your requirements, including:

    - Duration-based caching
    - Vary by query string
    - Vary by header
    - Custom cache key policies
    - Cache tag invalidation

If you need more advanced caching scenarios, you can also implement a custom IOutputCacheStore to use a different caching backend like Redis:
```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "your-redis-connection-string";
    options.InstanceName = "your-instance-name";
});

builder.Services.AddOutputCache()
    .AddStackExchangeRedisOutputCache();

```
## Difference between the different Caching

1. **Output Cache**:

- Caches the entire HTTP response (headers and body)
- Server-side caching mechanism
 - Specifically designed for caching web API responses and MVC action results
- Supports cache invalidation using tags
- Can vary cache by query parameters, headers, or custom logic
- Available in ASP.NET Core 7.0 and later

2. IMemoryCache (In-Memory Cache):
 - Stores data in memory of the local server
 - Best for single-server applications
 - Cache is lost when app restarts
 - Can cache any type of data (not just HTTP responses)
 - More granular control over what is cached
 - Memory is limited by server resources

3. IDistributed Cache
    - Supports different backing stores (Redis, SQL Server, NCache)
    - Suitable for multi-server/cloud applications
    - Cache persists across app restarts
    - Can cache any serializable data
    - Requires additional infrastructure
    - Higher latency compared to in-memory cache
    - Supports distributed scenarios

**Key Considertation in Choosing**:

1. Use OutputCache when:
    - You need to cache entire HTTP responses
    - You want built-in cache validation
    - You need to vary cache by HTTP parameters
    - You want simple configuration

2. Use IMemoryCache when:
    - You have a single-server application
    - You need to cache small amounts of data
    - You need fastest possible access
    - You want to cache arbitrary objects

3. Use IDistributedCache when:
    - You have a multi-server environment
    - You need cache persistence
    - You need to share cache between multiple servers
    - You need scalable caching solution




