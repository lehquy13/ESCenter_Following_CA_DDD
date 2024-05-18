using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;

public record GetCourseRequestDetailQuery(Guid CourseId)
    : IQueryRequest<CourseRequestForDetailDto>, IAuthorizationRequest;

public class GetCourseRequestDetailByTutorIdQueryValidator : AbstractValidator<GetCourseRequestDetailQuery>
{
    public GetCourseRequestDetailByTutorIdQueryValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
    }
}