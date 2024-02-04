using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Specifications.Tutors;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Accounts.Queries.GetTutorProfile;

public class GetTutorProfileQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper,
    ITutorRepository tutorRepository,
    ICurrentUserService currentUserService
)
    : QueryHandlerBase<GetTutorProfileQuery, TutorForProfileDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<TutorForProfileDto>> Handle(GetTutorProfileQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if(string.IsNullOrEmpty(currentUserService.CurrentUserId))
            {
                return Result.Fail(AccountServiceError.UnauthorizedError);
            }
            
            // This query includes verification information, major information, requests getting class
            var tutor = await tutorRepository
                .GetAsync(new TutorProfileSpec(
                    IdentityGuid.Create(new Guid(currentUserService.CurrentUserId))),
                    cancellationToken);

            if (tutor is null)
            {
                return Result.Fail(AccountServiceError.NonExistTutorError);
            }

            return Mapper.Map<TutorForProfileDto>(tutor);
        }
        catch (Exception ex)
        {
            return Result.Fail($"{AccountServiceError.FailToGetTutorProfileWithException} {ex.Message}");
        }
    }
}