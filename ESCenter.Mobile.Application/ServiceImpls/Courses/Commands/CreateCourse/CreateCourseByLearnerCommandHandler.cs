using ESCenter.Domain;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.CreateCourse;

public class CreateCourseByLearnerCommandHandler(
    IPublisher publisher,
    ICourseRepository courseRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IAppLogger<CreateCourseByLearnerCommandHandler> logger)
    : CommandHandlerBase<CreateCourseByLearnerCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(CreateCourseByLearnerCommand byLearnerCommand,
        CancellationToken cancellationToken)
    {
        var course = Course.Create(
            byLearnerCommand.CourseForLearnerCreateDto.Title,
            byLearnerCommand.CourseForLearnerCreateDto.Description,
            byLearnerCommand.CourseForLearnerCreateDto.LearningMode,
            byLearnerCommand.CourseForLearnerCreateDto.Fee,
            byLearnerCommand.CourseForLearnerCreateDto.Fee,
            Currency.USD,
            byLearnerCommand.CourseForLearnerCreateDto.GenderRequirement,
            byLearnerCommand.CourseForLearnerCreateDto.AcademicLevelRequirement,
            byLearnerCommand.CourseForLearnerCreateDto.LearnerGender,
            byLearnerCommand.CourseForLearnerCreateDto.LearnerName,
            byLearnerCommand.CourseForLearnerCreateDto.NumberOfLearner,
            byLearnerCommand.CourseForLearnerCreateDto.ContactNumber,
            byLearnerCommand.CourseForLearnerCreateDto.MinutePerSession,
            null,
            byLearnerCommand.CourseForLearnerCreateDto.SessionPerWeek,
            byLearnerCommand.CourseForLearnerCreateDto.Address,
            SubjectId.Create(byLearnerCommand.CourseForLearnerCreateDto.SubjectId),
            currentUserService.IsAuthenticated ? CustomerId.Create(currentUserService.UserId) : null);

        //Handle publish event to notification service
        await courseRepository.InsertAsync(course, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}