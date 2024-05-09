using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Middlewares;

internal sealed class GlobalExceptionHandler(
    IServiceProvider serviceProvider
) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
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

        httpContext.Response.Redirect("/admin/home/error");

        return new ValueTask<bool>(true);
    }
}