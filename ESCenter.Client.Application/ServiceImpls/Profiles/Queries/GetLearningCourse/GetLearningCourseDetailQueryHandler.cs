﻿using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Client.Application.ServiceImpls.Courses;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetLearningCourse;

public class GetLearningCourseDetailQueryHandler(
    ICourseRepository courseRepository,
    ISubjectRepository subjectRepository,
    IUserRepository userRepository,
    ITutorRepository tutorRepository,
    IIdentityRepository identityRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper
)
    : QueryHandlerBase<GetLearningCourseDetailQuery, CourseDetailDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<CourseDetailDto>> Handle(GetLearningCourseDetailQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var courseRequestQueryable =
                from courseFromDb in courseRepository.GetAll()
                join subjectFromDb in subjectRepository.GetAll() on courseFromDb.SubjectId equals subjectFromDb.Id
                join tutor1 in tutorRepository.GetAll() on courseFromDb.TutorId equals tutor1.Id
                join tutor in userRepository.GetAll() on tutor1.UserId equals tutor.Id
                join identityFromDb in identityRepository.GetAll() on tutor.Id equals identityFromDb.Id
                where courseFromDb.TutorId == IdentityGuid.Create(currentUserService.UserId) &&
                      courseFromDb.Id == CourseId.Create(request.CourseId)
                select new
                {
                    Course = courseFromDb,
                    Subject = subjectFromDb,
                    TutorInfo = tutor,
                    Identity = identityFromDb
                };

            var course =
                await asyncQueryableExecutor.SingleOrDefault(courseRequestQueryable, false, cancellationToken);

            if (course is null)
            {
                return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
            }

            var classDto = Mapper.Map<CourseDetailDto>(course);

            return classDto;
        }
        catch (Exception ex)
        {
            Logger.LogError("{Message} {Detail}", ex.Message, ex.InnerException?.Message ?? string.Empty);
            return Result.Fail(ex.Message);
        }
    }
}