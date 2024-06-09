using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.EntityFrameworkCore.Configs;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(nameof(Payment));
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .HasConversion(
                id => id.Value,
                value => PaymentId.Create(value)
            );

        builder.Property(r => r.TutorId)
            .HasColumnName(nameof(Payment.TutorId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TutorId.Create(value)
            );

        builder.HasOne<Tutor>()
            .WithMany()
            .HasForeignKey(nameof(Payment.TutorId))
            .IsRequired();

        builder.Property(r => r.CourseId)
            .HasColumnName(nameof(Payment.CourseId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CourseId.Create(value)
            );

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(nameof(Payment.CourseId))
            .IsRequired();

        builder.Property(x => x.PaymentStatus)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired();
    }
}