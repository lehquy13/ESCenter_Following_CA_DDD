using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Subjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Subjects.Queries.GetSubjects;

public class GetAllSubjectsQueryHandler(
    ISubjectRepository subjectRepository,
    IAppLogger<GetAllSubjectsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetAllSubjectsQuery, List<SubjectDto>>(logger, mapper)
{
    public override async Task<Result<List<SubjectDto>>> Handle(GetAllSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await subjectRepository.GetAllListAsync(cancellationToken);

        var subjectDtos = Mapper.Map<List<SubjectDto>>(subjects);
        return subjectDtos;
    }
}