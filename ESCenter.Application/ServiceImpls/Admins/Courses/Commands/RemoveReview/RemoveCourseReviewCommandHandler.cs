using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Courses.Commands.RemoveReview;

public class RemoveCourseReviewCommandHandler(
    ICourseRepository courseRepository,
    ITutorRepository tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<RemoveCourseReviewCommandHandler> logger)
    : CommandHandlerBase<RemoveCourseReviewCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(RemoveCourseReviewCommand command, CancellationToken cancellationToken)
    {
        var courseFromDb = await courseRepository.GetAsync(CourseId.Create(command.CourseId), cancellationToken);
        
        if (courseFromDb is  null)
        {
            return Result.Fail(CourseAppServiceErrors.CourseDoesNotExist);
        }

        courseFromDb.RemoveReview();
        
        // TODO: Should we split the tutor's rate updating into domain event?
        
        var tutorToUpdateRateQuery =
            from tutorQ in tutorRepository.GetAll()
            join courseQ in courseRepository.GetAll() on tutorQ.Id equals courseQ.TutorId into courseQs
            where tutorQ.Id == courseFromDb.TutorId
            select new
            {
                Tutor = tutorQ,
                ReviewRate = courseQs.Select(x => x.Review.Rate),
            };

        var tutorToUpdate = await asyncQueryableExecutor.FirstOrDefaultAsync(tutorToUpdateRateQuery, true,
            cancellationToken);
            
        if(tutorToUpdate is null)
        {
            Logger.LogError("Doesnt have any tutor to update rate");
            return Result.Fail(CourseAppServiceErrors.TutorNotExistsError);
        }
            
        tutorToUpdate.Tutor.UpdateRate(tutorToUpdate.ReviewRate.Average(x => x));


        if (await UnitOfWork.SaveChangesAsync(cancellationToken) > 0)
        {
            return Result.Fail("Fail to remove the review");
        }

        return Result.Success();
    }
}