using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.PurchaseCourse;

public class PurchaseCourseCommandHandler(
    ICourseRepository courseRepository,
    ITutorRepository tutorRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<PurchaseCourseCommandHandler> logger)
    : CommandHandlerBase<PurchaseCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(PurchaseCourseCommand command, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(CourseId.Create(command.CourseId), cancellationToken);

        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        var tutor = await tutorRepository
            .GetTutorByUserId(CustomerId.Create(currentUserService.UserId));

        if (tutor is null)
        {
            return Result.Fail(CourseAppServiceErrors.TutorDoesNotExist);
        }

        var result = course.Purchase(tutor.Id);

        if (result.IsFailure || await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(result.Error);
        }

        return Result.Success();
    }
}