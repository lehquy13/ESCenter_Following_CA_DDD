using System.Reflection;
using ESCenter.Admin.Application.ServiceImpls.DashBoards;
using ESCenter.Application;
using ESCenter.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Admin.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddBaseApplication(typeof(DependencyInjection).Assembly);

        services.AddScoped<IDashboardServices, DashboardServices>();

        return services;
    }

    public static IEnumerable<Assembly> GetApplicationCoreAssemblies =>
    [
        typeof(DependencyInjection).Assembly,
        typeof(BaseApplicationDependencyInjection).Assembly,
        typeof(DomainDependencyInjection).Assembly
    ];
}