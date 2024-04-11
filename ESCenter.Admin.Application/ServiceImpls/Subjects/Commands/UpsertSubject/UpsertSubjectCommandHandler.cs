using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Subjects.Commands.UpsertSubject;

public class UpsertSubjectCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<UpsertSubjectCommandHandler> logger,
    IMapper mapper)
    : CommandHandlerBase<UpsertSubjectCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(UpsertSubjectCommand request, CancellationToken cancellationToken)
    {
        var subjectExists =
            await subjectRepository.GetAsync(SubjectId.Create(request.SubjectDto.Id), cancellationToken);
        if (subjectExists is not null)
        {
            // Update
            subjectExists.SetDescription(request.SubjectDto.Description);
            subjectExists.SetName(request.SubjectDto.Name);
        }
        else
        {
            // Insert
            var subject = mapper.Map<Subject>(request.SubjectDto);
            await subjectRepository.InsertAsync(subject, cancellationToken);
        }

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(SubjectAppServiceError.FailToAddSubjectErrorWhileSavingChanges);
        }

        return Result.Success();
    }
}