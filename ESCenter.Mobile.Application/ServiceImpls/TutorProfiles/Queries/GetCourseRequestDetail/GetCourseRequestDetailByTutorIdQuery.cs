using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetCourseRequestDetail;

public record GetCourseRequestDetailByTutorIdQuery(Guid CourseRequestId)
    : IQueryRequest<CourseRequestForDetailDto>, IAuthorizationRequest;

public class GetCourseRequestDetailByTutorIdQueryValidator : AbstractValidator<GetCourseRequestDetailByTutorIdQuery>
{
    public GetCourseRequestDetailByTutorIdQueryValidator()
    {
        RuleFor(x => x.CourseRequestId).NotEmpty();
    }
}