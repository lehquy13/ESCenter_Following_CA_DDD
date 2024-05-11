using ESCenter.Persistence.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ESCenter.Persistence;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        
        return app;
    }
}