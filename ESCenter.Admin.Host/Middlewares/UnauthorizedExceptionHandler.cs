using Matt.SharedKernel;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Admin.Host.Middlewares;

internal sealed class UnauthorizedExceptionHandler(
    IServiceProvider serviceProvider
) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UnauthorizedException unauthorizedException)
        {
            return ValueTask.FromResult(false);
        }

        using (var scope = serviceProvider.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<IAppLogger<UnauthorizedExceptionHandler>>();

            logger.LogError(
                "Exception occurred: {Message}",
                unauthorizedException.Message);
        }


        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Detail = unauthorizedException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        return ValueTask.FromResult(true);
    }
}