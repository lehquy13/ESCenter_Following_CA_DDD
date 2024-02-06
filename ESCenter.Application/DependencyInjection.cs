using ESCenter.Application.Contract.Interfaces.DashBoards;
using ESCenter.Application.Mapping;
using ESCenter.Application.ServiceImpls;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddApplicationMappings();
            services.AddScoped<IDashboardServices, DashboardServices>();
            return services;
        }
    }
}