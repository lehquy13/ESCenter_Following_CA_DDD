using System.Reflection;
using ESCenter.Application.Behaviors;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Matt.SharedKernel.Application.Validations;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBaseApplication(this IServiceCollection services, Assembly mappingAssembly)
        {
            services.AddLazyCache();
            services
                .AddMediatorBehavior(mappingAssembly)
                .AddMediator(mappingAssembly)
                .AddApplicationMappings(mappingAssembly);

            return services;
        }

        public static IServiceCollection AddSharedKernel(this IServiceCollection services, Assembly applicationAssembly)
        {
            services
                .AddMediator(applicationAssembly)
                .AddApplicationMappings(applicationAssembly);

            return services;
        }

        private static IServiceCollection AddMediatorBehavior(this IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(LoggingPipelineBehavior<,>));
            
            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(applicationAssembly);
            return services;
        }
        private static IServiceCollection AddMediator(this IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(applicationAssembly);
                    cfg.NotificationPublisher = new TaskWhenAllPublisher();
                });

            return services;
        }

        private static IServiceCollection AddApplicationMappings(this IServiceCollection services,
            Assembly applicationAssembly)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(applicationAssembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}