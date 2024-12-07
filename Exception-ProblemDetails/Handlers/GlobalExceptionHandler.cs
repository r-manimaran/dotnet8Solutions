using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Exception_ProblemDetails.Handlers;
// internal sealed class GlobalExeptionHandler:IExceptionHandler
internal sealed class GlobalExeptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
                HttpContext httpContext,
                Exception exception,
                CancellationToken cancellationToken)
    {

        httpContext.Response.StatusCode = exception switch 
        {
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError          
        };
        Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Type = exception.GetType().Name,
                Title = "An error occured in the Application.",
                Detail = exception.Message,
                
               // Move this to common place, if in case there are multiple ExceptionHandler
               /* Instance = httpContext.Request.Method + " " + httpContext.Request.Path,
                Extensions = new Dictionary<string, object?>()
                {
                    {"requestId", httpContext.TraceIdentifier},
                    {"traceId", activity?.Id},
                    {"spanId", activity?.SpanId.ToString()}
                }*/
            }
        });

        /* await httpContext.Response.WriteAsJsonAsync(
        new ProblemDetails
        {
            Type = exception.GetType().Name,
            Title="An error occured in the Application.",
            Detail = exception.Message
        }, 
         cancellationToken); 
        return true;*/
    }
}