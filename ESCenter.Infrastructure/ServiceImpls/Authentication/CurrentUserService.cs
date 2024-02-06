using System.Security.Claims;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Microsoft.AspNetCore.Http;

namespace ESCenter.Infrastructure.ServiceImpls.Authentication;

internal class CurrentUserService : ICurrentUserService
{
    public string? CurrentUserId { get; }
    public bool IsAuthenticated { get; }
    public string? CurrentUserEmail { get; }
    public string? CurrentUserFullName { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            CurrentUserId = userId.Value;
            IsAuthenticated = true;
            CurrentUserEmail = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            CurrentUserFullName = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}