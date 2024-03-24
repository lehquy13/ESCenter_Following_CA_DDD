using ESCenter.Domain.Aggregates.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.EntityFrameworkCore.Configs;

internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification));
        builder.HasKey(r => r.Id);
        builder.Property(r => r.ObjectId).IsRequired();
        builder.Property(r => r.IsRead).IsRequired();
        builder.Property(r => r.Message).IsRequired();
        builder.Property(r => r.NotificationType).IsRequired();
    }
}