using System.Reflection;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using MediatR;

namespace Matt.SharedKernel.Application.Authorizations;

public class AuthorizationBehavior<TRequest, TResponse>(
    ICurrentUserService currentUserService
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Check if the request is an inheritor of IAuthorizationRequest  
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var requiredPermissions = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? [])
            .ToList();

        var requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        var requiredPolicies = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Policies?.Split(',') ?? [])
            .ToList();

        AuthorizeCurrentUser(requiredRoles, requiredPermissions);

        return await next();
    }

    private void AuthorizeCurrentUser(
        IEnumerable<string> requiredRoles,
        IEnumerable<string> requiredPermissions)
    {
        // Check if the current user has the required permissions 
        if (requiredPermissions.Except(currentUserService.Permissions).Any())
        {
            throw new UnauthorizedException("User is missing required permissions for taking this action");
        }

        // Check if the current user has the required roles 
        if (requiredRoles.Except(currentUserService.Roles).Any())
        {
            throw new UnauthorizedException("User is missing required roles for taking this action");
        }
    }
}