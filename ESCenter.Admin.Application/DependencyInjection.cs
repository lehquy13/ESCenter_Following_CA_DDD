using System.Reflection;
using ESCenter.Admin.Application.DashBoards;
using ESCenter.Admin.Application.ServiceImpls;
using ESCenter.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Admin.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddApplicationMappings();
        services.AddScoped<IDashboardServices, DashboardServices>();
        return services;
    }

    public static Assembly[] GetApplicationCoreAssemblies =>
    [
        typeof(DependencyInjection).Assembly,
        typeof(ESCenter.Domain.DependencyInjection).Assembly
    ];
}