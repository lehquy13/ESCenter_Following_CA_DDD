using ESCenter.Domain.Aggregates.Subscribers;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.Entity_Framework_Core.Configs;

internal class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable(nameof(Subscriber));
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.SubscriberId)
            .HasColumnName(nameof(Subscriber.SubscriberId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.SubscriberId)
            .IsRequired();
    }
}