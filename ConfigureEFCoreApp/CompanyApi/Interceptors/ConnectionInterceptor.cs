using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace CompanyApi.Interceptors;

public class ConnectionInterceptor : DbConnectionInterceptor
{
    private readonly ILogger<ConnectionInterceptor> _logger;

    public ConnectionInterceptor(ILogger<ConnectionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
        DbConnection connection, 
        ConnectionEventData eventData, 
        InterceptionResult result, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Opening connection to database:{connection.Database}");
        return result;
    }

    public override async ValueTask<InterceptionResult> ConnectionClosingAsync(
        DbConnection connection, 
        ConnectionEventData eventData, 
        InterceptionResult result)
    {
        _logger.LogInformation($"Closing connection to database:{connection.Database}");
        return result;
    }
}
