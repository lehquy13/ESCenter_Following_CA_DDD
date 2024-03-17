using System.Reflection;
using ESCenter.Application;
using ESCenter.Application.Accounts.Queries.Login;
using ESCenter.Application.Contracts.Authentications;
using FluentValidation;
using Matt.SharedKernel.Application.Validations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Client.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddBaseApplication(typeof(DependencyInjection).Assembly);
            services.AddTransient<IPipelineBehavior<LoginQuery,AuthenticationResult>, LoggingPipelineBehavior<LoginQuery,AuthenticationResult>>();
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