using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using Mapster;

namespace ESCenter.Mobile.Application.Contracts.Courses.Dtos;

public class CourseRequestForDetailDto
{
    public Guid Id { get; set; }

    public string TutorName { get; set; } = string.Empty;
    public string TutorPhone { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;
    public Guid TutorId { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RequestStatus { get; set; } = "Verifying";
    public string LearnerName { get; set; } = string.Empty;
    public string LearnerContact { get; set; } = string.Empty;
}

public class CourseRequestForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Course, CourseRequest, Subject, User, IdentityUser), CourseRequestForDetailDto>()
            .Map(dest => dest.RequestStatus, src => src.Item2.RequestStatus.ToString())
            .Map(dest => dest.LearnerContact, src => src.Item1.ContactNumber)
            .Map(dest => dest.LearnerName, src => src.Item1.LearnerName)
            .Map(dest => dest.Title, src => src.Item1.Title)
            .Map(dest => dest.SubjectName, src => src.Item3.Name)
            .Map(dest => dest.TutorName, src => src.Item4.GetFullName())
            .Map(dest => dest.TutorPhone, src => src.Item5.PhoneNumber)
            .Map(dest => dest.TutorEmail, src => src.Item5.Email)
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest, src => src);
    }
}