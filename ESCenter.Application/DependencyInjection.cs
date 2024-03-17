using System.Reflection;
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
            services
                .AddMediator(mappingAssembly)
                .AddMediatorBehavior()
                .AddApplicationMappings(mappingAssembly)
                .AddValidatorsFromAssembly(mappingAssembly)
                .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly)
                .AddLazyCache();

            return services;
        }

        public static IServiceCollection AddSharedKernel(this IServiceCollection services, Assembly applicationAssembly)
        {
            services
                .AddMediator(applicationAssembly)
                .AddApplicationMappings(applicationAssembly);

            return services;
        }

        private static IServiceCollection AddMediatorBehavior(this IServiceCollection services)
        {
            // services.AddScoped(
            //     typeof(IPipelineBehavior<,>),
            //     typeof(AuthorizationBehavior<,>));

            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(LoggingPipelineBehavior<,>));

            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

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