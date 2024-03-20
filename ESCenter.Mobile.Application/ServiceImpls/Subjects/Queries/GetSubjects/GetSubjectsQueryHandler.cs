using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.Subjects.Queries.GetSubjects;

public class GetSubjectsQueryHandler(
    ISubjectRepository subjectRepository,
    IAppLogger<GetSubjectsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetSubjectsQuery, List<SubjectDto>>(logger, mapper)
{
    public override async Task<Result<List<SubjectDto>>> Handle(GetSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await subjectRepository.GetListAsync(cancellationToken);

        var subjectDtos = Mapper.Map<List<SubjectDto>>(subjects);
        return subjectDtos;
    }
}