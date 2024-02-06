using ESCenter.Application.Contract.Courses.Dtos;
using ESCenter.Application.Contract.Notifications;
using ESCenter.Domain.Aggregates.CourseRequests;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.CourseRequests;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Shared.NotificationConsts;
using Mapster;

namespace ESCenter.Application.Mapping;

public class CourseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Notification, NotificationDto>()
            .Map(des => des.DetailPath, src =>
                src.NotificationType == NotificationEnum.CourseRequest
                    ? $"/{src.NotificationType.ToString()}/Edit/{src.ObjectId}"
                    : $"/{src.NotificationType.ToString()}/Detail?id={src.ObjectId}")
            .Map(des => des, src => src);

        //Config for Request getting class
        config.NewConfig<Course, LearningCourseForListDto>()
            .Map(dest => dest.LearningMode, src => src.LearningMode.ToString())
            .Map(dest => dest, src => src);

        config.NewConfig<CourseRequest, CourseRequestDto>();
        config.NewConfig<CourseForLearnerCreateDto, Course>()
            .Map(dest => dest.AcademicLevelRequirement, src => src.AcademicLevelRequirement)
            .Map(dest => dest.SubjectId, src => SubjectId.Create(src.SubjectId))
            .Map(dest => dest.LearnerId, src => IdentityGuid.Create(src.LearnerId))
            .Map(dest => dest, src => src);

        config.NewConfig<Course, CourseForDetailDto>()
            .Map(dest => dest.CourseRequestForDetailDtos, src => src.CourseRequests)
            .Map(dest => dest, src => src);

        config.NewConfig<(string, CourseRequest, string), CourseRequestForListDto>()
            .Map(dest => dest.SubjectName, src => src.Item3)
            .Map(dest => dest.Title, src => src.Item1)
            .Map(dest => dest.RequestStatus, src => src.Item2.RequestStatus.ToString())
            .Map(dest => dest.CourseId, src => src.Item2.CourseId.Value);

        config.NewConfig<(Course, CourseRequest, Subject, User, IdentityUser), CourseRequestForDetailDto>()
            .Map(dest => dest.RequestStatus, src => src.Item2.RequestStatus.ToString())
            .Map(dest => dest.LearnerContact, src => src.Item1.ContactNumber)
            .Map(dest => dest.LearnerName, src => src.Item1.LearnerName)
            .Map(dest => dest.Title, src => src.Item1.Title)
            .Map(dest => dest.SubjectName, src => src.Item3.Name)
            .Map(dest => dest.TutorName, src => src.Item4.GetFullName())
            .Map(dest => dest.TutorPhone, src => src.Item5.PhoneNumber)
            .Map(dest => dest.TutorEmail, src => src.Item5.Email)
            .Map(dest => dest, src => src);


        config.NewConfig<(Course, Subject, User, IdentityUser), CourseForDetailDto>()
            .Map(dest => dest.LearnerName, src => src.Item1.LearnerName)
            .Map(dest => dest.Title, src => src.Item1.Title)
            .Map(dest => dest.SubjectName, src => src.Item2.Name)
            .Map(dest => dest.TutorName, src => src.Item3.GetFullName())
            .Map(dest => dest.TutorPhoneNumber, src => src.Item4.PhoneNumber)
            .Map(dest => dest.TutorEmail, src => src.Item4.Email)
            .Map(dest => dest, src => src);

        config.NewConfig<CourseForCreateDto, Course>()
            .ConstructUsing(x =>
                Course.Create(
                    x.Title,
                    x.Description,
                    x.LearningMode.ToEnum<LearningMode>(),
                    x.Fee,
                    x.ChargeFee,
                    "Dollar",
                    x.GenderRequirement.ToEnum<Gender>(),
                    x.AcademicLevelRequirement.ToEnum<AcademicLevel>(),
                    x.LearnerGender,
                    x.LearnerName,
                    x.NumberOfLearner,
                    x.ContactNumber,
                    x.MinutePerSession,
                    null,
                    x.SessionPerWeek,
                    x.Address,
                    SubjectId.Create(x.SubjectId),
                    x.LearnerId.HasValue ? IdentityGuid.Create(x.LearnerId.Value) : null)
            );

        config.NewConfig<CourseForLearnerCreateDto, Course>()
            .ConstructUsing(x =>
                Course.Create(
                    x.Title,
                    x.Description,
                    x.LearningMode.ToEnum<LearningMode>(),
                    x.Fee,
                    x.Fee,
                    "Dollar",
                    x.GenderRequirement.ToEnum<Gender>(),
                    x.AcademicLevelRequirement.ToEnum<AcademicLevel>(),
                    x.LearnerGender,
                    x.LearnerName,
                    x.NumberOfLearner,
                    x.ContactNumber,
                    x.MinutePerSession,
                    null,
                    x.SessionPerWeek,
                    x.Address,
                    SubjectId.Create(x.SubjectId),
                    IdentityGuid.Create(x.LearnerId))
            );

        config.NewConfig<CourseUpdateDto, Course>()
            .Map(dest => dest.LearningMode, src => src.LearningMode.ToEnum<LearningMode>())
            .Map(dest => dest.GenderRequirement, src => src.GenderRequirement.ToEnum<Gender>())
            .Map(dest => dest.AcademicLevelRequirement, src => src.AcademicLevelRequirement.ToEnum<AcademicLevel>())
            .Map(dest => dest.SectionFee, src => Fee.Create(src.Fee, Currency.USD))
            .Map(dest => dest.ChargeFee, src => Fee.Create(src.ChargeFee, Currency.USD))
            .Map(dest => dest.SessionDuration, src => SessionDuration.Create(src.MinutePerSession, null))
            .Map(dest => dest.SessionPerWeek, src => SessionPerWeek.Create(src.SessionPerWeek))
            .Map(dest => dest.SubjectId, src => SubjectId.Create(src.SubjectId))
            .Map(dest => dest.TutorId, src => src.TutorId != Guid.Empty ? IdentityGuid.Create(src.TutorId) : null)
            .Map(dest => dest.Status, src => src.TutorId == Guid.Empty ? src.Status.ToEnum<Status>() : Status.Confirmed)
            .Map(dest => dest, src => src);


// ----------------------------------------------------------------------------------------------------------------------------

        config.NewConfig<Subject, SubjectDto>();
        config.NewConfig<SubjectDto, Subject>();
    }
}