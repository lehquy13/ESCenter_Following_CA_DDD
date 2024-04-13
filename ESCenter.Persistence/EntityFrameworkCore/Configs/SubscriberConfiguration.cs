using ESCenter.Domain.Aggregates.Subscribers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.EntityFrameworkCore.Configs;

internal class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable(nameof(Subscriber));
        builder.HasKey(r => r.Id);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);
    }
}