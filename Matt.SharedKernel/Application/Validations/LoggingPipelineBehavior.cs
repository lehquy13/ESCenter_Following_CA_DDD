using System.Diagnostics;
using System.Text.Json;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace Matt.SharedKernel.Application.Validations;

public class LoggingPipelineBehavior<TRequest, TResponse>(
    IAppLogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

        var body = request.GetType().GetProperties()
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(request)?.ToString() ?? "null");

        logger.LogInformation("Handling Body:\n{RequestBody}",
            JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true }));

        var sw = Stopwatch.StartNew();
        var val = await next();

        logger.LogInformation(
            "Handled {RequestName} with {Response} in {Ms} ms",
            typeof(TRequest).Name,
            val!,
            sw.ElapsedMilliseconds);
        sw.Stop();

        return val;
    }
}