using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.DomainEvents;
using ESCenter.Domain.Aggregates.Tutors;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Domain.EventualConsistency;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.ReviewCourse;

public class CourseReviewedDomainEventHandler(
    ICourseRepository courseRepository,
    ITutorRepository tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<CourseReviewedDomainEventHandler> logger)
    : INotificationHandler<CourseReviewedDomainEvent>
{
    public async Task Handle(CourseReviewedDomainEvent notification, CancellationToken cancellationToken)
    {
        var tutorToUpdateRateQuery =
            from tutorQ in tutorRepository.GetAll()
            join courseQ in courseRepository.GetAll() on tutorQ.Id
                equals courseQ.TutorId into courseQs
            where tutorQ.Id == notification.Course.TutorId
            select new
            {
                Tutor = tutorQ,
                ReviewRate = courseQs
                    .Where(x => x.Review != null)
                    .Select(x => x.Review!.Rate),
            };

        var tutorToUpdate = await asyncQueryableExecutor.FirstOrDefaultAsync(tutorToUpdateRateQuery,
            true,
            cancellationToken);

        if (tutorToUpdate is null)
        {
            logger.LogError("Doesnt have any tutor to update rate");
            throw new EventualConsistencyException(CourseAppServiceErrors.TutorNotExistsError);
        }

        if (tutorToUpdate.ReviewRate.Count() >= 2)
        {
            tutorToUpdate.Tutor.UpdateRate(tutorToUpdate.ReviewRate.Average(x => x));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}