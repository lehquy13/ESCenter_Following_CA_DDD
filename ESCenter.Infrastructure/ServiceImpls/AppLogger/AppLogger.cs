using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ESCenter.Infrastructure.ServiceImpls.AppLogger;

//TODO: Logging is going wrong
public class AppLogger<TCategory>(ILoggerFactory loggerFactory) : IAppLogger<TCategory>
{
    private readonly ILogger<TCategory> _logger = loggerFactory.CreateLogger<TCategory>();

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation("{Message} \n {Agrs}", message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning("{Message} \n {Agrs}", message, args);

    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError("{Message} \n {Agrs}", message, args);
    }
}