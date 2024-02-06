using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetVerification;

public class GetVerificationsByTutorIdQueryHandler(
    ITutorRepository tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper)
    : QueryHandlerBase<GetVerificationsByTutorIdQuery, TutorVerificationInfoForEditDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<TutorVerificationInfoForEditDto>> Handle(GetVerificationsByTutorIdQuery request,
        CancellationToken cancellationToken)
    {
        //Get tutor verification info
        var query =
            tutorRepository
                .GetAll()
                .Where(x => x.Id == IdentityGuid.Create(request.TutorId))
                .Select(x => new
                {
                    TutorVerificationInfos = x.TutorVerificationInfos,
                    ChangeVerificationRequests = x.ChangeVerificationRequests
                });

        var queryResult = await asyncQueryableExecutor.FirstOrDefaultAsync(query, true, cancellationToken);

        if (queryResult is null)
        {
            return Result.Fail(TutorAppServiceError.NonExistTutorError);
        }

        var result = new TutorVerificationInfoForEditDto()
        {
            TutorId = request.TutorId.ToString(),
            TutorVerificationInfoDtos = Mapper.Map<List<TutorVerificationInfoDto>>(queryResult.TutorVerificationInfos),
            ChangeVerificationRequestDtos = Mapper.Map<List<ChangeVerificationRequestDto>>(queryResult.ChangeVerificationRequests)
        };

        return result;
    }
}