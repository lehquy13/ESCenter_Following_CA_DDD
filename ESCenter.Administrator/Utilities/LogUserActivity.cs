using FluentEmail.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ESCenter.Administrator.Utilities;

public class LogUserActivity(ISender mediator) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        var id = resultContext.HttpContext.User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
        if(id is null) return;
        var userId = new Guid(id);
    }
}