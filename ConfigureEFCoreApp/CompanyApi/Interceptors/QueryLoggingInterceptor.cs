using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace CompanyApi.Interceptors;

public class QueryLoggingInterceptor : IDbCommandInterceptor
{
    private readonly ILogger<QueryLoggingInterceptor> _logger;

    public QueryLoggingInterceptor(ILogger<QueryLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public async ValueTask<InterceptionResult<DbDataReader>> ReadExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing SQL:{sql}",command.CommandText);
        return result;
    }

    public async ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken= default)
    {
        _logger.LogInformation($"Execution completed in {eventData.Duration.TotalMilliseconds} ms");
        return result;
    }
}
