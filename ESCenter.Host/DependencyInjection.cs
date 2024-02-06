﻿using System.Reflection;
using ESCenter.Application;
using ESCenter.Infrastructure;
using ESCenter.Persistence;
using Matt.AutoDI;
using Matt.SharedKernel.DependencyInjections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddHost(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        Assembly[] assemblies =
        {
            typeof(ESCenter.Domain.DependencyInjection).Assembly,
            typeof(ESCenter.Application.DependencyInjection).Assembly,
            typeof(ESCenter.Infrastructure.DependencyInjection).Assembly,
            typeof(ESCenter.Persistence.DependencyInjection).Assembly,
        };
        services.AddServiced(assemblies);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddCors();

        services
            .AddSharedKernel(typeof(ESCenter.Application.DependencyInjection).Assembly)
            .AddPersistence(configuration)
            .AddInfrastructure(configuration)
            .AddApplication();

        return services;
    }
}