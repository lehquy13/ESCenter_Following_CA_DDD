using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Errors;
using ESCenter.Domain.Specifications.Subjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Application.ServiceImpls.Accounts.Commands.UpdateTutorProfile;

public class UpdateTutorProfileCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    ITutorRepository tutorRepository,
    IUserRepository userRepository,
    IRepository<Subject, SubjectId> subjectRepository,
    IMapper mapper,
    IAsyncQueryableExecutor asyncQueryableExecutor)
    : CommandHandlerBase<UpdateTutorProfileCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpdateTutorProfileCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var tutorAsQueryable =
                from tutorFromDb in tutorRepository.GetAll()
                join userFromDb in userRepository.GetAll()
                    on tutorFromDb.Id equals userFromDb.Id
                select new
                {
                    Tutor = tutorFromDb,
                    User = userFromDb
                };

            var tutorQueryFromDb = await asyncQueryableExecutor.FirstOrDefaultAsync(tutorAsQueryable, true, cancellationToken);

            //Check if the subject existed
            if (tutorQueryFromDb is null)
            {
                return Result.Fail(UserError.NonExistTutorError);
            }

            mapper.Map(command.TutorBasicForUpdateDto, tutorQueryFromDb);

            var subjectListAboutToUpdate = await subjectRepository
                .GetListAsync(new SubjectListByNameSpec(command.TutorBasicForUpdateDto.Majors), cancellationToken);

            foreach (var major in tutorQueryFromDb.Tutor.TutorMajors)
            {
                if (subjectListAboutToUpdate.All(update => update.Id != major.SubjectId))
                {
                    tutorQueryFromDb.Tutor.RemoveMajor(major);
                }
            }

            // Now current major only contains the subjects that are in the subjectListAboutToUpdate
            // Then we add new majors to the tutor
            var addMajors = subjectListAboutToUpdate
                .Where(x => tutorQueryFromDb.Tutor.TutorMajors.All(update => update.SubjectId != x.Id))
                .Select(x => TutorMajor.Create(tutorQueryFromDb.Tutor.Id, x.Id, x.Name))
                .ToList();

            foreach (var major in addMajors)
            {
                tutorQueryFromDb.Tutor.AddTutorMajor(major);
            }

            if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
            {
                return Result.Fail(AccountServiceError.FailToUpdateTutorErrorWhileSavingChanges);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(AccountServiceError.FailToUpdateTutorError(ex.InnerException?.Message ?? ex.Message));
        }
    }
}