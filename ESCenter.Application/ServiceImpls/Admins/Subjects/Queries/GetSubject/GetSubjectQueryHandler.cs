using ESCenter.Application.Contract.Courses.Dtos;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Subjects.Queries.GetSubject;

public class GetSubjectQueryHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<GetSubjectQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetSubjectQuery, SubjectDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<SubjectDto>> Handle(GetSubjectQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await subjectRepository.GetAsync(SubjectId.Create(request.Id),cancellationToken);

        var subjectDtos = Mapper.Map<SubjectDto>(subjects);
        return subjectDtos;
    }
}