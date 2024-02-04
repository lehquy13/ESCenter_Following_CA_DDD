using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorMajors;

public class GetTutorMajorsQueryHandler(
    ITutorRepository tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<GetTutorMajorsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTutorMajorsQuery, List<TutorMajorDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<List<TutorMajorDto>>> Handle(GetTutorMajorsQuery request,
        CancellationToken cancellationToken)
    {
        var subjectsAsQueryable =
            from tutor in tutorRepository.GetAll()
            where tutor.Id == IdentityGuid.Create(request.TutorId)
            select new
            {
                Subject = tutor.TutorMajors
            };

        var subjectsList = await asyncQueryableExecutor.ToListAsync(subjectsAsQueryable, false, cancellationToken);

        var subjectDtos = Mapper.Map<List<TutorMajorDto>>(subjectsList);
        return subjectDtos;
    }
}