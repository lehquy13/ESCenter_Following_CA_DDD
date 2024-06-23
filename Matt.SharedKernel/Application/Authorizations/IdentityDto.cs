namespace Matt.SharedKernel.Application.Authorizations;

public record IdentityDto(Guid Id, string UserName, string Email, IList<string> Roles, string? Tenant = null);