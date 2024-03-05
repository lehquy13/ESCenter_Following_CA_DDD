using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Subjects.Commands.DeleteSubject;

public class DeleteSubjectCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<DeleteSubjectCommandHandler> logger)
    : CommandHandlerBase<DeleteSubjectCommand>(unitOfWork, logger)
{
    public override async Task<Result> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var subjectExists =
            await subjectRepository.GetAsync(SubjectId.Create(request.SubjectId), cancellationToken);
        if (subjectExists is null)
        {
            return Result.Fail(SubjectAppServiceError.NonExistSubjectError);
        }

        subjectExists.SoftDelete();

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(SubjectAppServiceError.FailToAddSubjectErrorWhileSavingChanges);
        }

        return Result.Success();
    }
}