using Microsoft.EntityFrameworkCore;
using Orders.Api.Data;
using Orders.Api.Models;
using Orders.Api.Repositories;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Channels;

namespace Orders.Api.Services;

internal sealed class WebhookDispatcherUsingDB
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WebhookDbContext _dbContext;
    private readonly Channel<WebhookDispatch> _webhookChannel;

    public WebhookDispatcherUsingDB(IHttpClientFactory httpClientFactory,
                                    WebhookDbContext dbContext,
                                    Channel<WebhookDispatch> webhookChannel)
    {
        _httpClientFactory = httpClientFactory;
        _dbContext = dbContext;
        _webhookChannel = webhookChannel;
    }

    public async Task DispatchAsync<T>(string eventType, T data) where T : notnull
    {
        using Activity? activity = Activity.Current?.Source.StartActivity($"{eventType} - Dispatch Webhook");
        activity?.SetTag("EventType", eventType);
        // Write to the channel, instead of earlier implementation of dispatching the webhook payload as part
        // of Create order Post request.
        await _webhookChannel.Writer.WriteAsync(new WebhookDispatch(eventType, data, activity?.Id));
    }

    public async Task ProcessAsync<T>(string eventType, T data)
    {
        var subscriptions = await _dbContext.WebhookSubscriptions
                            .AsNoTracking()
                            .Where(sub=>sub.EventType == eventType)
                            .ToListAsync();

        foreach (WebhookSubscription subscription in subscriptions)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            var payload = new WebhookPayload<T>
            {
                Id = Guid.NewGuid(),
                EventType =subscription.EventType,
                SubscriptionId = subscription.Id,
                Timestamp = DateTime.UtcNow,
                Data = data
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            try
            {
                // Send to WebhookUrl
                var response = await httpClient.PatchAsJsonAsync(subscription.WebhookUrl, payload);
                var attempt = new WebhookDeliveryAttempt
                {
                    Id = Guid.NewGuid(),
                    WebhookSubscriptionId = subscription.Id,
                    Payload = jsonPayload,
                    ResponseStatusCode = (int)response.StatusCode,
                    Success = response.IsSuccessStatusCode,
                    Timestamp = DateTime.UtcNow,
                };

                _dbContext.WebhookDeliveryAttempts.Add(attempt);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                var attempt = new WebhookDeliveryAttempt
                {
                    Id = Guid.NewGuid(),
                    WebhookSubscriptionId = subscription.Id,
                    Payload = jsonPayload,
                    ResponseStatusCode = null,
                    Success = false,
                    Timestamp = DateTime.UtcNow,
                };
                _dbContext.WebhookDeliveryAttempts.Add(attempt);
                await _dbContext.SaveChangesAsync();
                
            }
        }
    }
}
