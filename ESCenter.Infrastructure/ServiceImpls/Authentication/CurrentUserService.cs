using System.Security.Claims;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Microsoft.AspNetCore.Http;

namespace ESCenter.Infrastructure.ServiceImpls.Authentication;

internal class CurrentUserService : ICurrentUserService
{
    public Guid UserId { get; }
    public bool IsAuthenticated { get; }
    public string? CurrentUserEmail { get; }
    public string? CurrentUserFullName { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userId != null)
        {
            UserId = new Guid(userId.Value);
            IsAuthenticated = UserId != Guid.Empty;
            CurrentUserEmail = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            CurrentUserFullName = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        }
    }

    public void Authenticated()
    {
        if(UserId == Guid.Empty)
        {
            throw new Exception("User is not authenticated");
        }
    }
}