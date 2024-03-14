﻿using ESCenter.Client.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Mapster;
using MapsterMapper;
using Matt.Paginated;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Queries.GetCourses;

public class GetCoursesQueryHandler(
    ICourseRepository courseRepository,
    IReadOnlyRepository<Subject, SubjectId> subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<GetCoursesQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetCoursesQuery, PaginatedList<CourseForListDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<PaginatedList<CourseForListDto>>> Handle(GetCoursesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            //Create a list of class query
            var courseQuery =
                from course in courseRepository.GetAll()
                    .OrderByDescending(x => x.CreationTime)
                    .Where(x => x.IsDeleted == false)
                join subject in subjectRepository.GetAll().Where(x => x.IsDeleted == false)
                    on course.SubjectId equals subject.Id
                select new 
                {
                    Course = course,
                    Subject = subject.Name
                };

            //Filter by Today | Verifying | Purchasing | All
            switch (request.CourseParams.Filter)
            {
                case "Today":
                    courseQuery = courseQuery.Where(x => x.Course.CreationTime >= DateTime.Today);
                    break;
                case "Verifying":
                    courseQuery = courseQuery.Where(x => x.Course.Status == Status.OnVerifying);
                    break;
                case "Purchasing":
                    courseQuery = courseQuery.Where(x => x.Course.Status == Status.OnPurchasing);
                    break;
            }

            //Filter by SubjectName if it is not null
            if (!string.IsNullOrWhiteSpace(request.CourseParams.SubjectName))
            {
                courseQuery = courseQuery.Where(x =>
                    x.Subject.ToLower().Contains(request.CourseParams.SubjectName.ToLower()));
            }

            var count = await asyncQueryableExecutor.CountAsync(courseQuery, cancellationToken);

            var classesQueryResult =
                await asyncQueryableExecutor.ToListAsync(
                    courseQuery
                        .Skip((request.CourseParams.PageIndex - 1) * request.CourseParams.PageSize)
                        .Take(request.CourseParams.PageSize), 
                    false, cancellationToken);

            //Get the class of the page
            var classInformationDtos = classesQueryResult.Select(classR => (classR.Course, classR.Subject).Adapt<CourseForListDto>()).ToList();

            return PaginatedList<CourseForListDto>.Create(classInformationDtos, request.CourseParams.PageIndex,
                request.CourseParams.PageSize, count);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}