namespace Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;

public interface ICurrentUserService
{
    Guid UserId { get; }
    List<string> Permissions { get; }
    List<string> Roles { get; }
    void Authenticated();
    string? CurrentUserEmail { get; }
    string? CurrentUserFullName { get; }
    string? CurrentTenant { get; }
    bool IsAuthenticated { get; }
}