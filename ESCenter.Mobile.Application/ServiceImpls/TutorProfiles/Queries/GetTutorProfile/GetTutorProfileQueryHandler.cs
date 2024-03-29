using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetTutorProfile;

public class GetTutorProfileQueryHandler(
    ICurrentUserService currentUserService,
    IReadOnlyRepository<Tutor, TutorId> tutorRepository,
    IReadOnlyRepository<Subject, SubjectId> subjectRepository,
    IMapper mapper,
    IAppLogger<GetTutorProfileQueryHandler> logger)
    : QueryHandlerBase<GetTutorProfileQuery, TutorMinimalBasicDto>(logger, mapper)
{
    public override async Task<Result<TutorMinimalBasicDto>> Handle(GetTutorProfileQuery request,
        CancellationToken cancellationToken)
    {
        var tutor = await tutorRepository.GetAsync(TutorId.Create(currentUserService.UserId), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        return Mapper.Map<TutorMinimalBasicDto>(tutor);
    }
}