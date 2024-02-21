using ESCenter.Application.Contracts.Courses.Dtos;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorMajors;

public class GetTutorMajorsQueryHandler(
    ITutorRepository tutorRepository,
    ISubjectRepository subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<GetTutorMajorsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTutorMajorsQuery, List<SubjectDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<List<SubjectDto>>> Handle(GetTutorMajorsQuery request,
        CancellationToken cancellationToken)
    {
        var majors = await tutorRepository.GetTutorMajors(TutorId.Create(request.TutorId), cancellationToken);

        var subjects = await subjectRepository.GetListAsync(cancellationToken);
        //var subjectsList = await asyncQueryableExecutor.FirstOrDefaultAsync(subjectsAsQueryable, false, cancellationToken);

        var subjectDtos = Mapper.Map<List<SubjectDto>>(subjects);
        return subjectDtos;
    }
}