using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Admin.Host.Middlewares;

internal sealed class GlobalExceptionHandler(
    IServiceProvider serviceProvider
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<IAppLogger<GlobalExceptionHandler>>();

            logger.LogError("Exception occurred: {Message}", exception.Message);
        }
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "ES Server error"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        //await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}