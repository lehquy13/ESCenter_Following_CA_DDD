using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Matt.SharedKernel.Application.Authorizations;
using Matt.SharedKernel.Application.Validations;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Application
{
    public static class BaseApplicationDependencyInjection
    {
        public static IServiceCollection AddBaseApplication(this IServiceCollection services, Assembly mappingAssembly)
        {
            services
                .AddMediator(mappingAssembly)
                .AddValidatorsFromAssembly(typeof(BaseApplicationDependencyInjection)
                    .Assembly) // Handle base validation of application layer
                .AddValidatorsFromAssembly(mappingAssembly) // Handle validation of specific application layer
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

        private static IServiceCollection AddMediator(this IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(applicationAssembly,
                        typeof(BaseApplicationDependencyInjection).Assembly,
                        typeof(Domain.DomainDependencyInjection).Assembly);
                    //cfg.NotificationPublisher = new TaskWhenAllPublisher();

                    cfg.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
                    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                    cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                });

            return services;
        }

        private static IServiceCollection AddApplicationMappings(this IServiceCollection services,
            Assembly applicationAssembly)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(applicationAssembly, typeof(BaseApplicationDependencyInjection).Assembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}