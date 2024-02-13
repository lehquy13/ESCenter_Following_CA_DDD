using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.Entity_Framework_Core.Configs;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable(nameof(IdentityRole));
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .HasConversion(
                id => id.Value,
                value => IdentityRoleId.Create(value)
            );        
        builder.Property(r => r.Name).IsRequired();
    }
}
