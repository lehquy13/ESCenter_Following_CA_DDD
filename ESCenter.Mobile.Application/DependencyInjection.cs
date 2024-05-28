using System.Reflection;
using ESCenter.Application;
using ESCenter.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Mobile.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddBaseApplication(typeof(DependencyInjection).Assembly);
            return services;
        }

        public static IEnumerable<Assembly> GetApplicationCoreAssemblies =>
        [
            typeof(DependencyInjection).Assembly,
            typeof(BaseApplicationDependencyInjection).Assembly,
            typeof(DomainDependencyInjection).Assembly
        ];
    }
    
    
}