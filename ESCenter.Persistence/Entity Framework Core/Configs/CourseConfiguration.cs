using ESCenter.Domain.Aggregates.CourseRequests.ValueObjects;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.CourseRequests;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESCenter.Persistence.Entity_Framework_Core.Configs;

internal class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        ConfigureCourse(builder);
        ConfigureReview(builder);
        ConfigureCourseRequest(builder);
    }

    private void ConfigureCourse(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable(nameof(Course));
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CourseId.Create(value)
            );

        builder.Property(r => r.Title)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(r => r.Description)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(r => r.Address)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(o => o.SubjectId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => SubjectId.Create(value));
        
        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(nameof(Course.SubjectId))
            .IsRequired();

        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.LearningMode).IsRequired();
        builder.Property(r => r.GenderRequirement).IsRequired();
        builder.Property(r => r.AcademicLevelRequirement).IsRequired();
        builder.Property(r => r.NumberOfLearner).IsRequired();

        // Learner info
        builder.Property(r => r.LearnerGender).IsRequired();
        builder.Property(r => r.LearnerName).IsRequired();
        builder.Property(r => r.ContactNumber).IsRequired();
        builder.Property(o => o.LearnerId) // TODO: Check if this is correct
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => IdentityGuid.Create(value));

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(nameof(Course.LearnerId))
            .IsRequired();

        builder.Property(o => o.TutorId) // TODO: Check if this is correct
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => IdentityGuid.Create(value));

        // builder.HasOne<Tutor>()
        //     .WithMany()
        //     .HasForeignKey(nameof(Course.TutorId))
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.NoAction);

        builder.Property(r => r.SessionDuration)
            .HasColumnName(nameof(Course.SessionDuration))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => SessionDuration.Create(value, SessionDurationUnit.Minute)
            );

        builder.Property(r => r.SessionPerWeek)
            .HasColumnName(nameof(Course.SessionPerWeek))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => SessionPerWeek.Create(value)
            );

        builder.OwnsOne(course => course.ChargeFee,
            navigationBuilder =>
            {
                navigationBuilder.Property(address => address.Amount)
                    .HasColumnName(nameof(Course.ChargeFee.Amount));
                navigationBuilder.Property(address => address.Currency)
                    .HasColumnName(nameof(Course.ChargeFee.Currency));
            });

        builder.OwnsOne(course => course.SectionFee,
            navigationBuilder =>
            {
                navigationBuilder.Property(address => address.Amount)
                    .HasColumnName(nameof(Course.SectionFee.Amount));
                navigationBuilder.Property(address => address.Currency)
                    .HasColumnName(nameof(Course.SectionFee.Currency));
            });
    }

    private void ConfigureReview(EntityTypeBuilder<Course> builder)
    {
        builder.OwnsOne(o => o.Review, ib =>
        {
            ib.ToTable(nameof(Review));
            ib.HasKey(x => x.Id);
            ib.WithOwner().HasForeignKey(nameof(Review.CourseId));
            ib.Property(r => r.Rate).IsRequired();
            ib.Property(r => r.Detail).IsRequired();
        });
    }

    private void ConfigureCourseRequest(EntityTypeBuilder<Course> builder)
    {
        builder.OwnsMany(o => o.CourseRequests, ib =>
        {
            ib.ToTable(nameof(CourseRequest));
            ib.HasKey(x => x.Id);
            ib.Property(r => r.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => CourseRequestId.Create(value)
                );
            ib.WithOwner().HasForeignKey(nameof(CourseRequest.CourseId));
            ib.Property(r => r.RequestStatus).IsRequired();

            ib.Property(r => r.TutorId)
                .HasColumnName(nameof(CourseRequest.TutorId))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => IdentityGuid.Create(value)
                );

            ib.HasOne<Tutor>()
                .WithMany()
                .HasForeignKey(nameof(CourseRequest.TutorId))
                .IsRequired();

            ib.Property(r => r.Description).HasMaxLength(128).IsRequired();
        });
    }
}