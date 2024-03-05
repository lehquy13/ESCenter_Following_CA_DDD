using System.Reflection;
using ESCenter.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Mobile.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddApplicationMappings();
            return services;
        }

        public static Assembly[] GetApplicationCoreAssemblies =>
        [
            typeof(DependencyInjection).Assembly,
            typeof(ESCenter.Domain.DependencyInjection).Assembly
        ];
    }
}