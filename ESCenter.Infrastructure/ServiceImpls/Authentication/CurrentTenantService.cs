using System.Security.Claims;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Microsoft.AspNetCore.Http;

namespace ESCenter.Infrastructure.ServiceImpls.Authentication;

public class CurrentTenantService(
        IHttpContextAccessor httpContextAccessor
    )
    : ICurrentTenantService
{
    public string GetTenantId()
    {
        var userId = httpContextAccessor
            .HttpContext?.User
            .FindFirst(ClaimTypes.Actor)?.Value ?? "";
        
        return userId;
    }
}