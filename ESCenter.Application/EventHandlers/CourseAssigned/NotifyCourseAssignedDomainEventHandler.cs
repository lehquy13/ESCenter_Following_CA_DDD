using ESCenter.Application.Interfaces;
using ESCenter.Domain.Aggregates.Courses.DomainEvents;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.DomainServices.Errors;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Domain.EventualConsistency;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using MediatR;

namespace ESCenter.Application.EventHandlers.CourseAssigned;

public class NotifyCourseAssignedDomainEventHandler(
    IEmailSender emailSender,
    ICustomerRepository customerRepository,
    IRepository<Payment, PaymentId> paymentRepository,
    IRepository<Notification, int> notificationRepository,
    IUnitOfWork unitOfWork,
    IFireBaseNotificationService fireBaseNotificationService,
    ICurrentUserService currentUserService,
    IAppLogger<NotifyCourseAssignedDomainEventHandler> logger
) : INotificationHandler<TutorAssignedDomainEvent>
{
    public async Task Handle(TutorAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var message = $"Course {domainEvent.Course.Title} has been assigned to you. " +
                      $"Please check your tutor dashboard for more details.";

        var tutorEmail = await customerRepository.GetTutorByTutorId(domainEvent.TutorId, cancellationToken);

        List<Notification> notifications =
        [
            Notification.Create(
                "Course Assigned",
                domainEvent.Course.Id.Value.ToString(),
                NotificationEnum.Course),
            Notification.Create(
                "Course was assigned to you",
                domainEvent.Course.Id.Value.ToString(),
                NotificationEnum.Course,
                currentUserService.UserId,
                domainEvent.TutorId.Value)
        ];

        await notificationRepository.InsertManyAsync(notifications, cancellationToken);

        if (await unitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            // Log error

            logger.LogError("Failed to save notification to database");

            return;
        }

        if (tutorEmail == null)
        {
            logger.LogWarning("Tutor email is null when assigning course: {CourseId}", domainEvent.Course.Id);

            return;
        }

        await emailSender.SendEmail(tutorEmail.Email, "Course Assigned", message);
        await fireBaseNotificationService.SendNotificationAsync("Course Assigned",
            $"Course {domainEvent.Course.Title} has been assigned to you. "
            , tutorEmail.FCMToken);

        await HandleNext(domainEvent, cancellationToken);
    }

    private async Task HandleNext(TutorAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        if (domainEvent.Course.Status == Status.OnProgressing)
        {
            var course = domainEvent.Course;

            if (course.Status != Status.OnProgressing || course.TutorId == null)
            {
                throw new EventualConsistencyException(DomainServiceErrors.InvalidCourseStatusForPayment);
            }

            var payment = Payment.Create(course.TutorId, course.Id, course.ChargeFee.Amount);

            await paymentRepository.InsertAsync(payment, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}