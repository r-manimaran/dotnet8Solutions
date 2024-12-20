using Orders.Api.Models;
using Orders.Api.Repositories;
using Orders.Api.Services;

namespace Orders.Api.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Orders", (InMemoryOrderRepository orderRepository) =>
            {
                var orders = orderRepository.GetAll();
                return Results.Ok(orders);
            }).WithTags("Orders");

            app.MapPost("/Order", async (CreateOrderRequest request, 
                                          InMemoryOrderRepository orderRepository,
                                          WebhookDispatcher webhookDispatcher) =>
            {
                var newOrder = new Order(
                    Id: Guid.NewGuid(),
                    CustomerName : request.CustomerName,
                    Amount: request.Amount,
                    CreatedAt: DateTime.UtcNow
                );

                orderRepository.AddOrder(newOrder);
                await webhookDispatcher.DispatchAsync("order.created", newOrder);
                return Results.Ok(newOrder);
            }).WithTags("Orders"); ;
        }
    }
}
