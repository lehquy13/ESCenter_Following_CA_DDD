using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Courses.Commands.ConfirmCourse;

// I'll mark this one as not following the pattern
public class ConfirmCourseCommandHandler(
    ICourseRepository courseRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<ConfirmCourseCommandHandler> logger
) : CommandHandlerBase<ConfirmCourseCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(ConfirmCourseCommand command, CancellationToken cancellationToken)
    {
        // Check if course exists
        var course = await courseRepository.GetAsync(CourseId.Create(command.CourseId), cancellationToken);

        if (course is null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        // Check if payment exists using course id
        var payment = await paymentRepository.GetByCourseIdAsync(course.Id, cancellationToken);

        if (payment is null)
        {
            return Result.Fail("Payment not found");
        }

        var paymentResult = payment.ConfirmPayment();

        if (paymentResult.IsFailure)
        {
            return paymentResult;
        }

        course.ConfirmedCourse();

        return Result.Success();
    }
}