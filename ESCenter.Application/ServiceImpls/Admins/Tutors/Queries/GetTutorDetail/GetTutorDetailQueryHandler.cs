using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Application.ServiceImpls.Clients.TutorProfiles;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using Mapster;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorDetail;

public class GetTutorDetailQueryHandler(
    IUserRepository userRepository,
    ITutorRepository tutorRepository,
    ISubjectRepository subjectRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetTutorDetailQueryHandler> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetTutorDetailQuery, TutorUpdateDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<TutorUpdateDto>> Handle(GetTutorDetailQuery request,
        CancellationToken cancellationToken)
    {
        var tutorDetailAsQueryable =
            from user in userRepository.GetAll()
            join tutor in tutorRepository.GetAll() on user.Id equals tutor.UserId
            where user.Id == IdentityGuid.Create(request.TutorId)
            select new
            {
                User = user,
                Tutor = tutor
            };

        var queryResult =
            await asyncQueryableExecutor.FirstOrDefaultAsync(tutorDetailAsQueryable, false, cancellationToken);

        var subjects = await subjectRepository.GetListAsync(cancellationToken);

        if (queryResult is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        var tutorForDetailDto = (queryResult.User, queryResult.Tutor).Adapt<TutorUpdateDto>();
        
        // filter out the subjects that the tutor is not majoring in
        var tutorMajors = tutorForDetailDto.Majors
            .Select(x => x.Id);
        
        subjects = subjects.Where(x => tutorMajors.All(m => m != x.Id.Value)).ToList();
        tutorForDetailDto.Majors.AddRange(Mapper.Map<List<SubjectMajorDto>>(subjects));

        return tutorForDetailDto;
    }
}