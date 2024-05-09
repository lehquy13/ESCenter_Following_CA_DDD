using System.Reflection;
using ESCenter.Admin.Application;
using ESCenter.Administrator.Middlewares;
using ESCenter.Infrastructure;
using ESCenter.Persistence;
using FluentEmail.Core;
using Matt.AutoDI;

//using Matt.SharedKernel.DependencyInjections;

namespace ESCenter.Administrator;

public static class DependencyInjection
{
    public static IServiceCollection AddHost(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        IList<Assembly> assemblies = [];
        assemblies.AddRange(Admin.Application.DependencyInjection.GetApplicationCoreAssemblies);
        assemblies.AddRange(new[]
        {
            typeof(Infrastructure.DependencyInjection).Assembly,
            typeof(Persistence.DependencyInjection).Assembly
        });

        services.AddServiced(assemblies.ToArray());

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddCors();

        services
            .AddPersistence(configuration)
            .AddInfrastructure(configuration)
            .AddApplication();
        services.AddProblemDetails();

        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<UnauthorizedExceptionHandler>();
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}