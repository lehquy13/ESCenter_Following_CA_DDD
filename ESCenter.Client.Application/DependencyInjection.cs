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

        public static Assembly[] GetApplicationCoreAssemblies =>
        [
            typeof(DependencyInjection).Assembly,
            typeof(ESCenter.Application.DependencyInjection).Assembly,
            typeof(ESCenter.Domain.DependencyInjection).Assembly
        ];
    }
    
    
}