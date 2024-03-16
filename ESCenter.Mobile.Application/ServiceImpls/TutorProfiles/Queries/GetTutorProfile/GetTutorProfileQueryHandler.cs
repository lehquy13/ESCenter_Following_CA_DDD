using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Mobile.Application.Contracts.Users.Tutors;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Mobile.Application.ServiceImpls.TutorProfiles.Queries.GetTutorProfile;

public class GetTutorProfileQueryHandler(
    ICurrentUserService currentUserService,
    ITutorRepository tutorRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAppLogger<GetTutorProfileQueryHandler> logger)
    : QueryHandlerBase<GetTutorProfileQuery, TutorMinimalBasicDto>(unitOfWork, logger, mapper)
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