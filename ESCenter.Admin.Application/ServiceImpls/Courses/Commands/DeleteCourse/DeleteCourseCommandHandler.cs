using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.DeleteCourse;

public class DeleteCourseCommandHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<DeleteCourseCommandHandler> logger)
    : CommandHandlerBase<DeleteCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(CourseId.Create(request.Id), cancellationToken);
        
        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        course.SoftDelete();
      
        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            // var defaultRequest = new GetAllClassInformationsQuery();
            // _cache.Remove(defaultRequest.GetType() + JsonConvert.SerializeObject(defaultRequest));
            return Result.Fail(CourseAppServiceErrors.DeleteCourseErrorWhileSavingChanges);
        }

        return Result.Success();
    }
}