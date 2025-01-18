
using Orders.Api.OpenTelemetry;
using System.Diagnostics;
using System.Threading.Channels;

namespace Orders.Api.Services;

/// <summary>
/// Backgrouund service to process the each subscriptions and dispatch the webhook
/// </summary>
internal sealed class WebhookProccesor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Channel<WebhookDispatch> _webhooksChannel;
    public WebhookProccesor(IServiceScopeFactory scopeFactory,
                            Channel<WebhookDispatch> webhooksChannel)
    {
        _scopeFactory = scopeFactory;
        _webhooksChannel = webhooksChannel;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach(WebhookDispatch dispatch in _webhooksChannel.Reader.ReadAllAsync(stoppingToken))
        {
            using Activity? activity = DiagnosticConfig.ActivitySource.StartActivity($"{dispatch.EventType} - Process Webhook",
                ActivityKind.Internal,
                parentId: dispatch.ParentActivityId);

            using IServiceScope scope = _scopeFactory.CreateScope();
           
            var dispatcher = scope.ServiceProvider.GetRequiredService<WebhookDispatcherUsingDB>();
            
            await dispatcher.ProcessAsync(dispatch.EventType, dispatch.Data);
        }
    }


}
