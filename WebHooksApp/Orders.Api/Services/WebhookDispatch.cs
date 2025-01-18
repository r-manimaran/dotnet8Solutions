namespace Orders.Api.Services;

public sealed record WebhookDispatch (string EventType, object Data, string? ParentActivityId);

