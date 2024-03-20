using ESCenter.Admin.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetVerification;

public class GetVerificationsByTutorIdQueryHandler(
    ITutorRepository tutorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper)
    : QueryHandlerBase<GetVerificationsByTutorIdQuery, VerificationEditDto>(logger, mapper)
{
    public override async Task<Result<VerificationEditDto>> Handle(GetVerificationsByTutorIdQuery request,
        CancellationToken cancellationToken)
    {
        //Get tutor verification info
        var query =
            tutorRepository
                .GetAll()
                .Where(x => x.Id == CustomerId.Create(request.TutorId))
                .Select(x => new
                {
                    VerificationInfos = x.Verifications, ChangeVerificationRequests = x.ChangeVerificationRequest
                });

        var queryResult = await asyncQueryableExecutor.FirstOrDefaultAsync(query, true, cancellationToken);

        if (queryResult is null || queryResult.ChangeVerificationRequests is null)
        {
            return Result.Fail(TutorAppServiceError.NonExistTutorError);
        }

        var result = new VerificationEditDto()
        {
            TutorId = request.TutorId,
            VerificationDtos = queryResult
                .VerificationInfos
                .Select(x => x.Image),
            ChangeVerificationRequestDtos =
                Mapper.Map<List<ChangeVerificationRequestDto>>(queryResult.ChangeVerificationRequests)
        };

        return result;
    }
}