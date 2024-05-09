using Matt.SharedKernel;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ESCenter.Administrator.Middlewares;

internal sealed class NotFoundExceptionHandler(
    IServiceProvider serviceProvider
) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return ValueTask.FromResult(false);
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

        //await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return ValueTask.FromResult(true);
    }
}