using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
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

    private static void ConfigureCourse(EntityTypeBuilder<Course> builder)
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
            .IsRequired();
        builder.Property(r => r.Description)
            .IsRequired();
        builder.Property(r => r.Address)
            .IsRequired();

        builder.Property(r => r.SubjectId)
            .HasColumnName(nameof(Course.SubjectId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => SubjectId.Create(value)
            );

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
            .HasForeignKey(nameof(Course.LearnerId));

        builder.Property(o => o.TutorId) // TODO: Check if this is correct
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => TutorId.Create(value));

        builder.HasOne<Tutor>()
            .WithMany()
            .HasForeignKey(nameof(Course.TutorId))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

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
                navigationBuilder.Property(chargeFee => chargeFee.Amount)
                    .HasColumnName("ChargeFee");
                navigationBuilder.Property(chargeFee => chargeFee.Currency)
                    .HasColumnName(nameof(Course.ChargeFee.Currency));
            });

        builder.OwnsOne(course => course.SectionFee,
            navigationBuilder =>
            {
                navigationBuilder.Property(sectionFee => sectionFee.Amount)
                    .HasColumnName("SectionFee");
                navigationBuilder.Property(sectionFee => sectionFee.Currency)
                    .HasColumnName(nameof(Course.SectionFee.Currency));
            });
    }

    private static void ConfigureReview(EntityTypeBuilder<Course> builder)
    {
        builder.OwnsOne(o => o.Review, ib =>
        {
            ib.ToTable(nameof(Review));
            ib.HasKey(x => x.Id);
            ib.Property(r => r.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ReviewId.Create(value)
                );
            ib.WithOwner().HasForeignKey(nameof(Review.CourseId));
            ib.Property(r => r.Rate).IsRequired();
            ib.Property(r => r.Detail).IsRequired();
        });
    }

    private static void ConfigureCourseRequest(EntityTypeBuilder<Course> builder)
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
                    value => TutorId.Create(value)
                );
            
            ib.HasOne<Tutor>()
                .WithMany()
                .HasForeignKey(nameof(CourseRequest.TutorId))
                .IsRequired();

            // ib.OwnsOne(cr => cr.TutorCourseRequest, ibb =>
            // {
            //     ibb.ToTable(nameof(Tutor));
            //     ibb.HasKey(x => x.Id);
            //     ibb.Property(r => r.Id)
            //         .HasColumnName(nameof(TutorCourseRequest.Id))
            //         .ValueGeneratedNever()
            //         .HasConversion(
            //             id => id.Value,
            //             value => TutorId.Create(value)
            //         );
            //
            //     ibb.WithOwner().HasForeignKey(nameof(TutorCourseRequest.CourseRequestId));
            //
            //     ibb.OwnsOne(cr => cr.TutorUserInfo, ibbb =>
            //     {
            //         
            //         ibbb.ToTable(nameof(User));
            //         ibbb.HasKey(x => x.Id);
            //         ibbb.Property(r => r.Id)
            //             .HasColumnName(nameof(TutorUserInfo.Id))
            //             .ValueGeneratedNever()
            //             .HasConversion(
            //                 id => id.Value,
            //                 value => IdentityGuid.Create(value)
            //             );
            //         ibbb.WithOwner().HasForeignKey(nameof(TutorUserInfo.TutorId));
            //     });
            // });

            ib.Property(r => r.Description).IsRequired();
        });
    }

    private static void ConfigureTutorCourseRequest(EntityTypeBuilder<Course> builder)
    {
        
    }

}