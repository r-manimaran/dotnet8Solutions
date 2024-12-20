﻿using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Api.Models;
using Orders.Api.Repositories;

namespace Orders.Api.Endpoints
{
    public static class WebhooksEndpoints
    {
        public static void MapWebhookEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("webhooks/subscriptions", (
                        CreateWebhookRequest request,
                        InMemorySubscriptionRepository subscriptionRepository) =>
            {
                var newSubscription = new WebhookSubscription(
                    Id: Guid.NewGuid(),
                    EventType: request.EventType,
                    WebhookUrl: request.WebhookUrl,
                    CreatedOnUtc: DateTime.UtcNow);

                subscriptionRepository.AddWebhookSubscription(newSubscription);
                return Results.Ok(newSubscription);
            })
              .WithTags("Subscriptions");
        }
    }
}