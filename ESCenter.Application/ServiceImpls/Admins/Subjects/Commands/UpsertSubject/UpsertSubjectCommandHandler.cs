using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Commands;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Subjects.Commands.UpsertSubject;

public class UpsertSubjectCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<UpsertSubjectCommandHandler> logger,
    IMapper mapper)
    : CommandHandlerBase<UpsertSubjectCommand>(unitOfWork, logger)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAppLogger<UpsertSubjectCommandHandler> _logger = logger;

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

        if (await _unitOfWork.SaveChangesAsync(cancellationToken) <= 0)
        {
            return Result.Fail(SubjectAppServiceError.FailToAddSubjectErrorWhileSavingChanges);
        }

        return Result.Success();
    }
}