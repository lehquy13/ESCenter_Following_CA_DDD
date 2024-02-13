using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.Entity_Framework_Core.Configs;

//TODO: learn how to config with owns many
internal class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.ToTable(nameof(IdentityUser));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName(nameof(IdentityUser.Id))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );

        // builder.HasOne(r => r.User)
        //     .WithOne(x => x.IdentityUser)
        //     .HasForeignKey<IdentityUser>(x => x.Id)
        //     .IsRequired();
        //
        builder.Property(r => r.IdentityRoleId)
            .HasColumnName(nameof(IdentityUser.IdentityRoleId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityRoleId.Create(value)
            );

        builder.Property(r => r.PasswordHash).IsRequired();
        builder.Property(r => r.PasswordSalt).IsRequired();

        builder.Property(r => r.UserName);
        builder.Property(r => r.NormalizedUserName);

        builder.Property(r => r.PasswordHash);

        //Email is unique
        builder.HasIndex(r => r.Email).IsUnique();
        builder.Property(r => r.Email);

        builder.Property(r => r.NormalizedEmail);
        builder.Property(r => r.EmailConfirmed);

        builder.Property(r => r.PhoneNumber);
        builder.Property(r => r.PhoneNumberConfirmed);

        builder.Property(r => r.ConcurrencyStamp).IsRequired();

        builder.OwnsOne(r => r.OtpCode,
            navigationBuilder =>
            {
                navigationBuilder.Property(address => address.Value)
                    .HasColumnName("OtpCode")
                    .HasMaxLength(6);
                navigationBuilder.Property(address => address.ExpiredTime)
                    .HasColumnName("ExpiredTime");
            });
    }
}