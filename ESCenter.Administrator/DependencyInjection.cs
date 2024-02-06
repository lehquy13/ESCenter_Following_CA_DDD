using ESCenter.Administrator.Mapping;

namespace ESCenter.Administrator;
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        return services;
    }
}

