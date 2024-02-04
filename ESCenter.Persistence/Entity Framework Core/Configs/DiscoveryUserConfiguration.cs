using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Domain.Aggregates.DiscoveryUsers.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.Entity_Framework_Core.Configs;

internal class DiscoveryUserConfiguration : IEntityTypeConfiguration<DiscoveryUser>
{
    public void Configure(EntityTypeBuilder<DiscoveryUser> builder)
    {
        builder.ToTable(nameof(DiscoveryUser));
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName(nameof(DiscoveryUser.Id))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => DiscoveryUserId.Create(value)
            );

        builder.Property(r => r.DiscoveryId)
            .HasColumnName(nameof(DiscoveryUser.DiscoveryId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => DiscoveryId.Create(value)
            );
        
        builder.HasOne<Discovery>()
            .WithMany()
            .HasForeignKey(nameof(DiscoveryUser.DiscoveryId))
            .IsRequired();

        builder.Property(r => r.UserId)
            .HasColumnName(nameof(DiscoveryUser.UserId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(nameof(DiscoveryUser.UserId))
            .IsRequired();
    }
}