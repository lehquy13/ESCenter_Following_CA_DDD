using ESCenter.Application.Contract.Courses.Dtos;
using ESCenter.Domain.Aggregates.Subjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Subjects.Queries.GetSubjects;

public class GetSubjectsQueryHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<GetSubjectsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetSubjectsQuery, List<SubjectDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<List<SubjectDto>>> Handle(GetSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await subjectRepository.GetListAsync(cancellationToken);

        var subjectDtos = Mapper.Map<List<SubjectDto>>(subjects);
        return subjectDtos;
    }
}