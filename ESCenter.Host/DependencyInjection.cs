using ESCenter.Application;
using ESCenter.Infrastructure;
using ESCenter.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddHost(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddCors();

        services
            .AddPersistence(configuration)
            .AddInfrastructure(configuration)
            .AddApplication();

        return services;
    }
}