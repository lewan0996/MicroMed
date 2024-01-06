using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Exceptions;

namespace Shared.API;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DomainException and not ObjectNotFoundException) 
            return false;

        switch (exception)
        {
            case DomainException ex:
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = ex.Message
                };

                httpContext.Response.StatusCode = problemDetails.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                return true;
            }
            case ObjectNotFoundException:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return true;
            default:
                return false;
        }
    }
}

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        => services.AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();
}