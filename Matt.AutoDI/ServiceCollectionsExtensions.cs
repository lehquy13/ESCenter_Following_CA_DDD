using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Matt.AutoDI;

public static class ServiceCollectionsExtensions
{
    #region Extensions

    /// <summary>
    /// Registers all items for given assemblies
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddServiced(this IServiceCollection services, params Assembly[] assemblies)
    {
        var compatibleAssemblies = FilterAssemblies(assemblies);

        // We dont have domain assembly reference, so that we can't handle DI for domain services
        var servicesToRegister = compatibleAssemblies
            .SelectMany(x => x.GetTypes())
            .Where(t => typeof(IServiced).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        foreach (var serviceToRegister in servicesToRegister)
        {
            var (serviceType, implementationType) = GetTypes(serviceToRegister);

            var lifetime = GetLifetime(serviceToRegister);

            if (typeof(IHasImplementationFactory).IsAssignableFrom(serviceToRegister))
            {
                RegisterWithImplementationFactory(services, implementationType, lifetime);
            }
            else
            {
                RegisterWithTypes(services, serviceType, implementationType, lifetime);
            }
        }

        return services;
    }


    /// <summary>
    /// Registers all items for calling assembly
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServicedForCallingAssembly(this IServiceCollection services)
    {
        var callingAssembly = Assembly.GetCallingAssembly();

        return AddServiced(services, callingAssembly);
    }

    #endregion

    #region Registration

    private static void RegisterWithTypes(IServiceCollection services, Type serviceType, Type implementationType,
        ServiceLifetime lifetime)
    {
        var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);

        services.Add(descriptor);
    }

    private static void RegisterWithImplementationFactory(IServiceCollection services, Type implementationType,
        ServiceLifetime lifetime)
    {
        var classInstance = Activator.CreateInstance(implementationType);
        var factory = implementationType
            .GetMethod(nameof(IHasImplementationFactory.GetFactory));
        if (factory is null)
            throw new InvalidOperationException($"Factory method is not defined for {implementationType.FullName}");

        var factory1 = factory.Invoke(classInstance, null);

        if (factory1 is null)
            throw new InvalidOperationException($"Factory method returned null for {implementationType.FullName}");

        var factoryDelegate = (Func<IServiceProvider, object>)factory1;
        var descriptor = new ServiceDescriptor(implementationType, factoryDelegate, lifetime);

        services.Add(descriptor);
    }

    #endregion Registration

    #region Helpers

    private static (Type serviceType, Type implementationType) GetTypes(Type serviceToRegister)
    {
        Type? genericInterface;

        // IDomainService, IUserRepo
        if (!serviceToRegister.IsGenericTypeDefinition)
        {
            genericInterface = serviceToRegister.GetInterfaces()
                .MaxBy(x => x.GetInterfaces().Length);

            return (genericInterface ?? serviceToRegister,
                serviceToRegister);
        }

        if (serviceToRegister.IsGenericType && typeof(IServiced).IsAssignableFrom(typeof(IOpenGenericService<>)))
        {
            genericInterface = serviceToRegister.GetInterfaces()
                .LastOrDefault(x => x.Name == typeof(IOpenGenericService<>).Name);

            return (genericInterface != null
                ? genericInterface.GetGenericArguments()[0].GetGenericTypeDefinition()
                : serviceToRegister, serviceToRegister.GetGenericTypeDefinition());
        }

        //Generic type like IRepository<T>, IReadOnlyRepository<T>
        genericInterface = serviceToRegister.GetInterfaces()
            .FirstOrDefault(x => x.IsGenericType && typeof(IServiced).IsAssignableFrom(x)); // this is the original

        return (genericInterface != null
            ? genericInterface
            : serviceToRegister, serviceToRegister);
    }

    private static ServiceLifetime GetLifetime(Type serviceToRegister)
    {
        var lifetime = ServiceLifetime.Transient;

        if (typeof(IScoped).IsAssignableFrom(serviceToRegister))
            lifetime = ServiceLifetime.Scoped;
        else if (typeof(ISingleton).IsAssignableFrom(serviceToRegister)) lifetime = ServiceLifetime.Singleton;

        return lifetime;
    }

    private static IEnumerable<Assembly> FilterAssemblies(params Assembly[] assemblies)
    {
        var currentAssembly = Assembly.GetAssembly(typeof(IServiced));

        return assemblies.Where(x => x.FullName != currentAssembly!.FullName);
    }

    #endregion Helpers
}