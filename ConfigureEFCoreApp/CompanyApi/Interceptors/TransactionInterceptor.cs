using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace CompanyApi.Interceptors;

public class TransactionInterceptor : IDbTransactionInterceptor
{
    private readonly ILogger<TransactionInterceptor> _logger;
    public TransactionInterceptor(ILogger<TransactionInterceptor> logger)
    {
        _logger = logger;
    }

    public async ValueTask<InterceptionResult<DbTransaction>> TransactionStartingAsync(
        DbConnection connection,
        TransactionStartingEventData eventData,
        InterceptionResult<DbTransaction> result,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Starting transaction on database:{connection.Database}");
        return result;
    }

    public async ValueTask<DbTransaction> TransactionStartedAsync(
       DbConnection connection,
       TransactionStartingEventData eventData,
       DbTransaction result,
       CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Transaction started with isolation level:{result.IsolationLevel}");
        return result;
    }
}
