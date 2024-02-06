using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Clients.TutorProfiles.Queries.GetTutorProfile;

public class GetTutorProfileQueryHandler(
    ITutorRepository tutorRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAppLogger<GetTutorProfileQueryHandler> logger)
    : QueryHandlerBase<GetTutorProfileQuery, TutorMinimalBasicDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<TutorMinimalBasicDto>> Handle(GetTutorProfileQuery request,
        CancellationToken cancellationToken)
    {
        var tutor = await tutorRepository.GetAsync(IdentityGuid.Create(request.TutorId), cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorProfileAppServiceError.NonExistTutorError);
        }

        return Mapper.Map<TutorMinimalBasicDto>(tutor);
    }
}