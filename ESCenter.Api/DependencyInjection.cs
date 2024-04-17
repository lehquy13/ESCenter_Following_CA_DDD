using System.Reflection;
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

        services.AddCors();

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