using System.Reflection;
using ESCenter.Admin.Application;
using ESCenter.Administrator.Middlewares;
using ESCenter.Infrastructure;
using ESCenter.Persistence;
using FluentEmail.Core;
using Matt.AutoDI;

namespace ESCenter.Administrator;

public static class DependencyInjection
{
    public static IServiceCollection AddHost(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddExceptionHandler()
            .RegisterByAutoDi();
        
        services
            .AddPersistence(configuration)
            .AddInfrastructure(configuration)
            .AddPresentation()
            .AddApplication();

        return services;
    }

    private static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();

        return services;
    }

    private static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddCors();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddProblemDetails();

        return services;
    }

    private static void RegisterByAutoDi(this IServiceCollection services)
    {
        IList<Assembly> assemblies = [];

        assemblies.AddRange(Admin.Application.DependencyInjection.GetApplicationCoreAssemblies);
        assemblies.AddRange([
            typeof(Infrastructure.DependencyInjection).Assembly,
            typeof(Persistence.DependencyInjection).Assembly
        ]);

        services.AddServiced(assemblies.ToArray());
    }
}