using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.EntityFrameworkCore.Configs;

internal class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable(nameof(Subject));
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .HasConversion(
                id => id.Value,
                value => SubjectId.Create(value)
            );

        builder.Property(r => r.Name)
            .IsRequired();

        builder.Property(r => r.Description)
            .IsRequired();
    }
}