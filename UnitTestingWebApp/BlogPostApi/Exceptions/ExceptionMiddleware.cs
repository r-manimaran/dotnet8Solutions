using System.ComponentModel.DataAnnotations;

namespace BlogPostApi.Exceptions;

public class ExceptionMiddleware 
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(ArgumentNullException ex)
        {
            _logger.LogError(ex, "Null argument error occurred");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new
            {
                Status = 400,
                Message = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Validation error occured");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new
            {
                Status = 400,
                Message = "Validation failed",
                Errors = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                Status = 500,
                Message = "An internal error Occured"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
