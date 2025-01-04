using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Net;

namespace Api1.Exceptions;

public class AppExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AppExceptionHandler> logger;

    public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
    {
        this.logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
                                          Exception exception,
                                          CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        logger.LogError("{exception}",exception.ToString());
        await httpContext.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = $"Custom Message: Internal Server Error for the IExceptionHandler . {Environment.NewLine} Exceptions:{exception.Message}"
        }.ToString(), cancellationToken: cancellationToken);
        
        return default;
    }
}
