using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace ESCenter.Infrastructure.ServiceImpls.AppLogger;

internal class AppLogger<TCategory>(ILoggerFactory serilogLogger) : IAppLogger<TCategory>
{
    private readonly ILogger<TCategory> _logger = serilogLogger.CreateLogger<TCategory>();

    public void LogInformation(string? message, params object?[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string? message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string? message, params object[] args)
    {
        _logger.LogError(message, args);
    }
}

internal class SerilogFactory
{
    public Logger SerilogLogger { get; } = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}