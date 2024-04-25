using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Matt.SharedKernel.Application.Validations;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace Matt.SharedKernel.DependencyInjections;

// public static class SharedKernelDependencyInjection
// {
//     public static IServiceCollection AddSharedKernel(this IServiceCollection services, Assembly applicationAssembly)
//     {
//         services
//             .AddMediator(applicationAssembly)
//             .AddApplicationMappings(applicationAssembly);
//
//         return services;
//     }
//
//     private static IServiceCollection AddMediator(this IServiceCollection services, Assembly applicationAssembly)
//     {
//         services.AddMediatR(
//             cfg =>
//             {
//                 cfg.RegisterServicesFromAssemblies(applicationAssembly);
//                 cfg.NotificationPublisher = new TaskWhenAllPublisher();
//             });
//
//         services.AddScoped(
//             typeof(IPipelineBehavior<,>),
//             typeof(ValidationBehavior<,>));
//
//         services.AddValidatorsFromAssembly(applicationAssembly);
//
//         return services;
//     }
//
//     private static IServiceCollection AddApplicationMappings(this IServiceCollection services, Assembly applicationAssembly)
//     {
//         var config = TypeAdapterConfig.GlobalSettings;
//         config.Scan(applicationAssembly);
//
//         services.AddSingleton(config);
//         services.AddScoped<IMapper, ServiceMapper>();
//
//         return services;
//     }
// }