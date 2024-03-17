using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses.Entities;
using FluentValidation;
using Mapster;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Queries.GetCourseRequest;

public record GetCourseRequestQuery(Guid CourseRequestId) : IQueryRequest<CourseRequestCancelDto>, IAuthorizationRequest;

public class GetCourseRequestQueryValidator : AbstractValidator<GetCourseRequestQuery>
{
    public GetCourseRequestQueryValidator()
    {
        RuleFor(x => x.CourseRequestId).NotEmpty();
    }
}

public class CourseRequestCancelDto
{
    public Guid CourseRequestId { get; set; }
    public Guid CourseId { get; set; }
    public string Description { get; set; } = "The course is unavailable now.";
}

public class CourseRequestCancelDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CourseRequest, CourseRequestCancelDto>()
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CourseId, src => src.CourseId.Value)
            .Map(dest => dest.CourseRequestId, src => src.Id.Value)
            .Map(dest => dest, src => src);
    }
}