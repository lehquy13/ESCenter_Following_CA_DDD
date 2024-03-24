using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.EntityFrameworkCore.Configs;

internal class DiscoveryConfiguration : IEntityTypeConfiguration<Discovery>
{
    private void ConfigureDiscovery(EntityTypeBuilder<Discovery> builder)
    {
        builder.ToTable(nameof(Discovery));
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .HasConversion(
                id => id.Value,
                value => DiscoveryId.Create(value)
            );
    }

    private void ConfigureDiscoverySubject(EntityTypeBuilder<Discovery> builder)
    {
        builder.OwnsMany(o => o.DiscoverySubjects, ib =>
        {
            ib.ToTable(nameof(DiscoverySubject));
            ib.HasKey(x => x.Id);
            ib.Property(r => r.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd()
                .HasConversion(
                    id => id.Value,
                    value => DiscoverySubjectId.Create(value)
                );
            
            ib.WithOwner().HasForeignKey(nameof(DiscoverySubject.DiscoveryId));
            
            ib.Property(r => r.SubjectId)
                .HasColumnName(nameof(DiscoverySubject.SubjectId))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => SubjectId.Create(value)
                );
            
            ib.HasOne<Subject>()
                .WithMany()
                .HasForeignKey(nameof(DiscoverySubject.SubjectId))
                .IsRequired();
        });
    }

    public void Configure(EntityTypeBuilder<Discovery> builder)
    {
        ConfigureDiscovery(builder);
        ConfigureDiscoverySubject(builder);
    }
}