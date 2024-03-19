using System.Reflection;
using ESCenter.Client.Application;
using ESCenter.Client.Host.Middlewares;
using ESCenter.Infrastructure;
using ESCenter.Persistence;
using FluentEmail.Core;
using Matt.AutoDI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Matt.SharedKernel.DependencyInjections;

namespace ESCenter.Client.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddHost(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        RegisterByAutoDi(services);

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddCors();

        services
            .AddPersistence(configuration)
            .AddInfrastructure(configuration)
            .AddApplication();
        services.AddProblemDetails();

        AddExceptionHandler(services);

        return services;
    }

    private static void AddExceptionHandler(IServiceCollection services)
    {
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
    }

    private static void RegisterByAutoDi(IServiceCollection services)
    {
        IList<Assembly> assemblies = [];
        assemblies.AddRange(Client.Application.DependencyInjection.GetApplicationCoreAssemblies);
        assemblies.AddRange(new[]
        {
            typeof(ESCenter.Infrastructure.DependencyInjection).Assembly,
            typeof(ESCenter.Persistence.DependencyInjection).Assembly
        });

        services.AddServiced(assemblies.ToArray());
    }
}