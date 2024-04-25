namespace Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;

public interface ICurrentUserService
{
    Guid UserId { get; }
    List<string> Permissions { get; }
    List<string> Roles { get; }
    bool IsAuthenticated { get; }
    string? CurrentUserEmail { get; }
    string? CurrentUserFullName { get; }
}