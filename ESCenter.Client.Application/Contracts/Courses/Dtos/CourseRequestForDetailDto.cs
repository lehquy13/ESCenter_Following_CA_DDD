using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Users;
using Mapster;

namespace ESCenter.Client.Application.Contracts.Courses.Dtos;

public class CourseRequestForDetailDto
{
    public Guid Id { get; init; }

    public string TutorName { get; init; } = string.Empty;
    public string TutorPhone { get; init; } = string.Empty;
    public string TutorEmail { get; init; } = string.Empty;
    public Guid TutorId { get; init; }
    public Guid CourseId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string SubjectName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string RequestStatus { get; init; } = "Verifying";
    public string LearnerName { get; init; } = string.Empty;
    public string LearnerContact { get; init; } = string.Empty;
}

public class CourseRequestForDetailDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Course, CourseRequest, Subject, Customer), CourseRequestForDetailDto>()
            .Map(dest => dest.RequestStatus, src => src.Item2.RequestStatus.ToString())
            .Map(dest => dest.LearnerContact, src => src.Item1.ContactNumber)
            .Map(dest => dest.LearnerName, src => src.Item1.LearnerName)
            .Map(dest => dest.Title, src => src.Item1.Title)
            .Map(dest => dest.SubjectName, src => src.Item3.Name)
            .Map(dest => dest.TutorName, src => src.Item4.GetFullName())
            .Map(dest => dest.TutorPhone, src => src.Item4.PhoneNumber)
            .Map(dest => dest.TutorEmail, src => src.Item4.Email)
            .Map(dest => dest.Id, src => src.Item1.Id.Value)
            .Map(dest => dest, src => src);
    }
}