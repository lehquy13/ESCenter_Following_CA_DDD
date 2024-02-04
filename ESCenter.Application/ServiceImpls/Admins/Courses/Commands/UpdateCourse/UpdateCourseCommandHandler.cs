using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(
    ICourseRepository courseRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IAppLogger<UpdateCourseCommandHandler> logger)
    : CommandHandlerBase<UpdateCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpdateCourseCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var course = await courseRepository
                .GetAsync(CourseId.Create(command.CourseUpdateDto.Id), cancellationToken);

            if (course is null)
            {
                return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
            }

            mapper.Map(command.CourseUpdateDto, course);

            if (command.CourseUpdateDto.TutorId != Guid.Empty)
            {
                foreach (var courseRequest in course.CourseRequests)
                {
                    courseRequest.Cancel();
                }
            }

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(CourseAppServiceErrors.UpdateCourseFailedWhileSavingChanges);
            }

            return Result.Success();

            // Clear cache
            // var defaultRequest = new GetAllClassInformationsQuery();
            // _cache.Remove(defaultRequest.GetType() + JsonConvert.SerializeObject(defaultRequest));
        }
        catch (Exception ex)
        {
            return Result.Fail("Error happens when class is updating." + ex.Message);
        }
    }
}