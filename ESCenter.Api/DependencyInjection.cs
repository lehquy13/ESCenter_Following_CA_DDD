using System.Reflection;
using ESCenter.Api.Middlewares;
using ESCenter.Infrastructure;
using ESCenter.Mobile.Application;
using ESCenter.Persistence;
using FluentEmail.Core;
using Matt.AutoDI;

namespace ESCenter.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        RegisterByAutoDi(services);
        AddMiddleware(services);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddCors();

        return services;
    }
    
    public static IServiceCollection AddMiddleware(this IServiceCollection services)
    {
        services.AddProblemDetails();

        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        return services;
    }

    private static void RegisterByAutoDi(IServiceCollection services)
    {
        IList<Assembly> assemblies = [];
        assemblies.AddRange(Mobile.Application.DependencyInjection.GetApplicationCoreAssemblies);
        assemblies.AddRange(new[]
        {
            typeof(Infrastructure.DependencyInjection).Assembly,
            typeof(Persistence.DependencyInjection).Assembly
        });

        services.AddServiced(assemblies.ToArray());
    }
}