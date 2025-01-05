using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace CompanyApi.Interceptors;

public class ErrorHandlingInterceptor : IDbCommandInterceptor
{
    private readonly ILogger<ErrorHandlingInterceptor> _logger;

    public ErrorHandlingInterceptor(ILogger<ErrorHandlingInterceptor> logger)
    {
        _logger = logger;
    }

    public async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing command:{command}", command.CommandText);
            throw;
        }
    }
}
