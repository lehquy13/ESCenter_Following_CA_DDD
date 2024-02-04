using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.Entity_Framework_Core.Configs;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );

        builder.Property(r => r.FirstName)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(r => r.LastName)
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(r => r.Email)
            .IsUnique();

        builder.Property(r => r.PhoneNumber)
            .HasMaxLength(15);

        builder.Property(r => r.Gender)
            .IsRequired();

        builder.Property(r => r.BirthYear)
            .IsRequired();

        builder.Property(r => r.Avatar)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasMaxLength(128)
            .IsRequired();

        builder.OwnsOne(user => user.Address,
            navigationBuilder =>
            {
                navigationBuilder.Property(address => address.Country)
                    .HasColumnName("Country");
                navigationBuilder.Property(address => address.City)
                    .HasColumnName("City");
            });
    }
}