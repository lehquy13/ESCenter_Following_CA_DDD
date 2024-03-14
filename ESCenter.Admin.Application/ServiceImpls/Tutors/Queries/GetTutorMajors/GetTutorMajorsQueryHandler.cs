using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Subjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorMajors;

/// <summary>
/// Mark as deprecated
/// </summary>
/// <param name="subjectRepository"></param>
/// <param name="unitOfWork"></param>
/// <param name="logger"></param>
/// <param name="mapper"></param>
public class GetTutorMajorsQueryHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<GetTutorMajorsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTutorMajorsQuery, List<SubjectDto>>(unitOfWork, logger, mapper)
{

    public override async Task<Result<List<SubjectDto>>> Handle(GetTutorMajorsQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await subjectRepository.GetListAsync(cancellationToken);

        var subjectDtos = Mapper.Map<List<SubjectDto>>(subjects);
        return subjectDtos;
    }
}