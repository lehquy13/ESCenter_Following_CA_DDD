using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.SetCoursePayment;

public class SetCoursePaymentCommandHandler(
    ITutorRepository tutorRepository,
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<SetCoursePaymentCommandHandler> logger
) : CommandHandlerBase<SetCoursePaymentCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(SetCoursePaymentCommand command, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(CourseId.Create(command.CourseId), cancellationToken);

        if (course is null)
        {
            return Result.Fail("Course not found");
        }

        var tutor = await tutorRepository.GetAsync(TutorId.Create(command.TutorId), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail("Tutor not found");
        }

        return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0
            ? Result.Fail("Fail to set tutor for course")
            : Result.Success();
    }
}