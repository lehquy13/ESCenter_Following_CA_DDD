using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Subjects.Queries.GetSubject;

public class GetSubjectQueryHandler(
    ISubjectRepository subjectRepository,
    IAppLogger<GetSubjectQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetSubjectQuery, SubjectDto>(logger, mapper)
{
    public override async Task<Result<SubjectDto>> Handle(GetSubjectQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await subjectRepository.GetAsync(SubjectId.Create(request.Id), cancellationToken);

        if (subjects is null)
        {
            return Result.Fail(SubjectAppServiceError.NonExistSubjectError);
        }

        var subjectDtos = Mapper.Map<SubjectDto>(subjects);
        return subjectDtos;
    }
}