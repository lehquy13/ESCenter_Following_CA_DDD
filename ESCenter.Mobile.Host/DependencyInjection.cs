using System.Reflection;
using ESCenter.Application;
using ESCenter.Host.Middlewares;
using ESCenter.Infrastructure;
using ESCenter.Mobile.Application;
using ESCenter.Persistence;
using FluentEmail.Core;
using Matt.AutoDI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ESCenter.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddHost(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        IList<Assembly> assemblies = [];
        assemblies.AddRange(Mobile.Application.DependencyInjection.GetApplicationCoreAssemblies);
        assemblies.AddRange(new[]
        {
            typeof(ESCenter.Infrastructure.DependencyInjection).Assembly,
            typeof(ESCenter.Persistence.DependencyInjection).Assembly
        });

        services.AddServiced(assemblies.ToArray());

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddCors();

        services
            //.AddSharedKernel(typeof(Mobile.Application.DependencyInjection).Assembly)
            .AddPersistence(configuration)
            .AddInfrastructure(configuration)
            .AddApplication();
        services.AddProblemDetails();

        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        return services;
    }
}