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
            var subject = Subject.Create(request.SubjectDto.Name, request.SubjectDto.Description);
            await subjectRepository.InsertAsync(subject);
        }

        return await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0 ? Result.Fail(SubjectAppServiceError.FailToAddSubjectErrorWhileSavingChanges) : Result.Success();
    }
}