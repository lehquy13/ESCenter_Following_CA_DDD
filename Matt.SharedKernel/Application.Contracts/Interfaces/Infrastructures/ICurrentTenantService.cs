namespace Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;

public interface ICurrentTenantService
{
    string GetTenantId();
}