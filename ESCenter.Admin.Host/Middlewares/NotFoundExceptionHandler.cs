using Matt.SharedKernel;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Admin.Host.Middlewares;

internal sealed class NotFoundExceptionHandler(
    IServiceProvider serviceProvider
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }

        using (var scope = serviceProvider.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<IAppLogger<NotFoundExceptionHandler>>();

            logger.LogError(
                "Exception occurred: {Message}",
                notFoundException.Message);
        }


        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = notFoundException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}