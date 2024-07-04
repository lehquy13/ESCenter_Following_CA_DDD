using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.SetCourseTutor;

public class SetCourseTutorCommandHandler(
    ICourseRepository courseRepository,
    ITutorRepository tutorRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<SetCourseTutorCommandHandler> logger
) : CommandHandlerBase<SetCourseTutorCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(SetCourseTutorCommand command, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(CourseId.Create(command.CourseId), cancellationToken);

        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        if (command.TutorId != Guid.Empty)
        {
            var tutor = await tutorRepository.GetTutorByUserId(CustomerId.Create(command.TutorId), cancellationToken);

            if (tutor is null)
            {
                return Result.Fail(CourseAppServiceErrors.TutorNotExistsError);
            }

            var result = course.AssignTutor(tutor.Id);
            if (result.IsFailure)
            {
                return result;
            }
        }
        else
        {
            course.UnAssignTutor();
        }

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}