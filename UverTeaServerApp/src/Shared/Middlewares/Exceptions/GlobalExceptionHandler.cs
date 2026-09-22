using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UverTeaServerApp.Shared.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, message) = exception switch
        {
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized, 
                "Unauthorized", 
                exception.Message
            ),
            
            FluentValidation.ValidationException validationEx => (
                StatusCodes.Status400BadRequest, 
                "Validation Error", 
                string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))
            ),

            ResourceNotFoundException => (
                StatusCodes.Status404NotFound, 
                "Not Found", 
                exception.Message
            ),
            
            DuplicateResourceException => (
                StatusCodes.Status409Conflict, 
                "Conflict", 
                exception.Message
            ),
            
            _ => (
                StatusCodes.Status500InternalServerError, 
                "Server Error", 
                "An internal server error occurred."
            )
        };

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = message,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}