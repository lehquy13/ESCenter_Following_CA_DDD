using ESCenter.Application.NotificationImpls;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.NotificationConsts;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Commands.CreateTutor;

public class CreateTutorCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    ITutorRepository tutorRepository,
    IUserRepository userRepository,
    ISubjectRepository subjectRepository,
    IMapper mapper,
    IPublisher publisher)
    : CommandHandlerBase<CreateTutorCommand>(unitOfWork,
        logger)
{
    public override async Task<Result> Handle(CreateTutorCommand command, CancellationToken cancellationToken)
    {
        try
        {
            //Register user as new tutor
            var userInformation = mapper.Map<User>(command.TutorForCreateUpdateDto.UserForCreateDto);
            await userRepository.InsertAsync(userInformation,
                cancellationToken);
            var tutorInformation = mapper.Map<Tutor>(command.TutorForCreateUpdateDto.TutorForCreateUpdateDto);

            var subjectIds = command.TutorForCreateUpdateDto.TutorForCreateUpdateDto.Majors.Select(SubjectId.Create);
            var subjects = await subjectRepository.GetListByIdsAsync(subjectIds, cancellationToken);

            // add new majors to tutor
            var tutorMajors =
                subjects
                    .Select(x => TutorMajor.Create(tutorInformation.Id, x.Id, x.Name))
                    .ToList();

            tutorInformation.UpdateAllMajor(tutorMajors);

            await tutorRepository.InsertAsync(tutorInformation, cancellationToken);

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(TutorAppServiceError.FailToCreateTutorWhileSavingChanges);
            }

            var message = $"New tutor: {userInformation.GetFullName()}  at {DateTime.Now.ToLongDateString()}";
            await publisher.Publish(
                new NewObjectCreatedEvent(userInformation.Id.Value.ToString(), message, NotificationEnum.Tutor),
                cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail("Error happens when tutor is adding or updating: " + ex.Message);
        }
    }
}