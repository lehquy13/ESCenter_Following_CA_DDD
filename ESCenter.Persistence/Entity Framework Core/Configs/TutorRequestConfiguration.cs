using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.TutorRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.Entity_Framework_Core.Configs;

internal class TutorRequestConfiguration : IEntityTypeConfiguration<TutorRequest>
{
    public void Configure(EntityTypeBuilder<TutorRequest> builder)
    {
        builder.ToTable(nameof(TutorRequest));
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName(nameof(TutorRequest.Id))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TutorRequestId.Create(value)
            );
        builder.Property(r => r.LearnerId)
            .HasColumnName(nameof(TutorRequest.LearnerId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value)
            );
        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(nameof(TutorRequest.LearnerId))
            .IsRequired();
        
        builder.Property(r => r.TutorId)
            .HasColumnName(nameof(TutorRequest.TutorId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TutorId.Create(value)
            );

        builder.HasOne<Tutor>()
            .WithMany()
            .HasForeignKey(nameof(TutorRequest.TutorId))
            .IsRequired();
        
        builder.Property(r => r.Message).IsRequired();
        builder.Property(r => r.RequestStatus).IsRequired();
    }
}