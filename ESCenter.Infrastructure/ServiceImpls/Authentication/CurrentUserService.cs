using System.Security.Claims;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ESCenter.Infrastructure.ServiceImpls.Authentication;

internal class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public Guid UserId { get; }
    public List<string> Permissions { get; }
    public List<string> Roles { get; }
    public bool IsAuthenticated { get; }
    public string? CurrentUserEmail { get; }
    public string? CurrentUserFullName { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IAppLogger<CurrentUserService> logger)
    {
        _httpContextAccessor = httpContextAccessor;

        try
        {
            var userId = GetSingleClaimValue(ClaimTypes.NameIdentifier);
            UserId = new Guid(userId);

            IsAuthenticated = UserId != Guid.Empty;
            CurrentUserEmail = GetSingleClaimValue(ClaimTypes.Email);
            CurrentUserFullName = GetSingleClaimValue(ClaimTypes.Name);
            Permissions = GetClaimValues("permissions");
            Roles = GetClaimValues(ClaimTypes.Role);
        }
        catch (Exception exception)
        {
            logger.LogError("Error occurred while getting user claims: {Message}", exception.Message);

            UserId = Guid.Empty;
            IsAuthenticated = false;
            CurrentUserEmail = null;
            CurrentUserFullName = null;
            Permissions = new List<string>();
            Roles = new List<string>();
        }
    }

    private List<string> GetClaimValues(string claimType) =>
        _httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();

    private string GetSingleClaimValue(string claimType) =>
        _httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;

    public void Authenticated()
    {
        if (UserId == Guid.Empty)
        {
            throw new Exception("User is not authenticated");
        }
    }
}