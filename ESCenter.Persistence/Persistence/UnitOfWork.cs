using ESCenter.Persistence.Entity_Framework_Core;
using Matt.Auditing;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace ESCenter.Persistence.Persistence;

internal sealed class UnitOfWork(
    ILogger<UnitOfWork> logger,
    AppDbContext appDbContext,
    ICurrentUserService currentUserService) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();

        logger.LogDebug("On save changes...");
        return await appDbContext.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IHasCreationTime>> hasCreationTimeEntries =
            appDbContext.ChangeTracker.Entries<IHasCreationTime>();

        foreach (var entityEntry in hasCreationTimeEntries)
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(e => e.CreationTime).CurrentValue = DateTime.Now;

                // If the entity is type of ICreationAuditedObject<T>, we should set CreatorId
                //TODO: The logic here may goes wrong
                if (entityEntry.Entity is ICreationAuditedObject)
                    entityEntry.Property("CreatorId").CurrentValue = currentUserService.UserId.ToString();
            }

        IEnumerable<EntityEntry<IHasModificationTime>> hasModificationTimeEnties =
            appDbContext.ChangeTracker.Entries<IHasModificationTime>();

        foreach (var entityEntry in hasModificationTimeEnties)
            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(e => e.LastModificationTime).CurrentValue = DateTime.Now;

                //TODO: The logic here may goes wrong
                if (entityEntry.Entity is IModificationAuditedObject)
                    entityEntry.Property("LastModifierId").CurrentValue = currentUserService.UserId.ToString();
            }
    }
}