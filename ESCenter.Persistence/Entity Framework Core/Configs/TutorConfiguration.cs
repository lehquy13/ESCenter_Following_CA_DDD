using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.Entity_Framework_Core.Configs;

internal class TutorConfiguration : IEntityTypeConfiguration<Tutor>
{
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        builder.ToTable(nameof(Tutor));
        ConfigureTutor(builder);
        ConfigureChangeVerificationRequest(builder);
        ConfigureVerification(builder);
        ConfigureTutorMajor(builder);
    }

    private static void ConfigureChangeVerificationRequest(EntityTypeBuilder<Tutor> builder)
    {
        builder.OwnsOne(o => o.ChangeVerificationRequest, ib =>
        {
            ib.ToTable(nameof(ChangeVerificationRequest));
            
            ib.HasKey(x => x.Id);
            ib.Property(r => r.Id)
                .HasColumnName(nameof(ChangeVerificationRequest.Id))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ChangeVerificationRequestId.Create(value)
                );
            
            ib.Property(r => r.TutorId)
                .HasColumnName(nameof(ChangeVerificationRequest.TutorId))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => TutorId.Create(value)
                );

            ib.Property(r => r.RequestStatus).IsRequired();

            ib.OwnsMany(cvr => cvr.ChangeVerificationRequestDetails, cvrd =>
            {
                cvrd.ToTable(nameof(ChangeVerificationRequestDetail));
                
                cvrd.HasKey(x => x.Id);
                cvrd.Property(r => r.Id)
                    .HasColumnName(nameof(ChangeVerificationRequestDetail.Id))
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ChangeVerificationRequestDetailId.Create(value)
                    );
                
                cvrd.WithOwner().HasForeignKey(x => x.ChangeVerificationRequestId);
                cvrd.Property(r => r.ImageUrl).IsRequired();
            });
        });
    }

    private static void ConfigureVerification(EntityTypeBuilder<Tutor> builder)
    {
        builder.OwnsMany(o => o.Verifications, ib =>
        {
            ib.ToTable(nameof(Verification));
            ib.HasKey(x => x.Id);
            ib.Property(r => r.Id)
                .HasColumnName(nameof(Verification.Id))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => VerificationId.Create(value)
                );
            ib.WithOwner().HasForeignKey(x => x.TutorId);
            ib.Property(x => x.Image).IsRequired();
        });
    }

    private static void ConfigureTutorMajor(EntityTypeBuilder<Tutor> builder)
    {
        builder.OwnsMany(o => o.TutorMajors, ib =>
        {
            ib.ToTable(nameof(TutorMajor));
            ib.HasKey(x => x.Id);
            ib.Property(r => r.Id)
                .HasColumnName(nameof(TutorMajor.Id))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => TutorMajorId.Create(value)
                );
            ib.WithOwner().HasForeignKey(x => x.TutorId);

            ib.Property(r => r.SubjectId)
                .HasColumnName(nameof(TutorMajor.SubjectId))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => SubjectId.Create(value)
                );

            ib.HasOne<Subject>()
                .WithMany()
                .HasForeignKey(nameof(TutorMajor.SubjectId))
                .IsRequired();
            ib.Property(r => r.SubjectName).IsRequired();
        });
    }

    private void ConfigureTutor(EntityTypeBuilder<Tutor> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TutorId.Create(value)
            );

        //Lack of User foreign key
        builder.Property(r => r.UserId)
            .HasColumnName(nameof(Tutor.UserId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );
        
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Tutor>(nameof(Tutor.UserId))
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(r => r.AcademicLevel).IsRequired();
        builder.Property(r => r.University).IsRequired();
        builder.Property(r => r.IsVerified).IsRequired();
        builder.Property(r => r.Rate).IsRequired();
    }
}