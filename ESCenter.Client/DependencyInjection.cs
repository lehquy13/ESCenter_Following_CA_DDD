using System.Reflection;
using ESCenter.Client.Application;
using ESCenter.Client.Middlewares;
using ESCenter.Infrastructure;
using ESCenter.Persistence;
using FluentEmail.Core;
using Matt.AutoDI;

namespace ESCenter.Client;

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
            typeof(Infrastructure.DependencyInjection).Assembly,
            typeof(Persistence.DependencyInjection).Assembly
        });

        services.AddServiced(assemblies.ToArray());
    }
}