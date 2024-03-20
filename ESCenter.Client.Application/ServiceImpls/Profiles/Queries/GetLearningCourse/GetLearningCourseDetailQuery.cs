﻿using ESCenter.Client.Application.Contracts.Courses.Dtos;
using FluentValidation;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;

public record GetLearningCourseDetailQuery(Guid CourseId) : IQueryRequest<LearningCourseDetailForClientDto>, IAuthorizationRequest;

public class GetLearningCourseDetailQueryValidator : AbstractValidator<GetLearningCourseDetailQuery>
{
    public GetLearningCourseDetailQueryValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
    }
}
