using System.Security.Claims;

namespace ESCenter.Administrator.Middlewares;

public class AuthorizeMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var httpContextAccessor = context.RequestServices.GetService<IHttpContextAccessor>();
        
        if(httpContextAccessor?.HttpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        var roleClaims = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);

        if(roleClaims is null || roleClaims.Value != "Admin")
        {
            context.Response.Redirect("/admin/authentication");
        }
        
        return next(context);
    }
}

