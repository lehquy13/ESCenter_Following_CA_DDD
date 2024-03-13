using System.Reflection;
using ESCenter.Admin.Application;
using ESCenter.Admin.Host.Middlewares;
using ESCenter.Infrastructure;
using ESCenter.Persistence;
using FluentEmail.Core;
using Matt.AutoDI;
//using Matt.SharedKernel.DependencyInjections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Admin.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddHost(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        IList<Assembly> assemblies = [];
        assemblies.AddRange(Admin.Application.DependencyInjection.GetApplicationCoreAssemblies);
        assemblies.AddRange(new[]
        {
            typeof(ESCenter.Infrastructure.DependencyInjection).Assembly,
            typeof(ESCenter.Persistence.DependencyInjection).Assembly
        });

        services.AddServiced(assemblies.ToArray());

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddCors();

        services
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