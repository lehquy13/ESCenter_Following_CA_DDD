using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace ESCenter.Infrastructure.ServiceImpls.AppLogger;

public class AppLogger<TCategory> : IAppLogger<TCategory>
{
    private readonly ILogger<TCategory> _logger;

    public AppLogger()
    {
        var microsoftLogger = new SerilogLoggerFactory(SerilogFactory.SerilogLogger)
            .CreateLogger<TCategory>();

        _logger = microsoftLogger;
    }

    public void LogInformation(string message, params object?[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }
}

internal static class SerilogFactory
{
    public static Logger SerilogLogger { get; }

    static SerilogFactory()
    {
        SerilogLogger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}