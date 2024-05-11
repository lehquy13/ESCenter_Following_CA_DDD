using System.Reflection;
using ESCenter.Application;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Client.Application
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
            typeof(ApplicationDependencyInjection).Assembly,
            typeof(Domain.DomainDependencyInjection).Assembly
        ];
    }
    
    
}