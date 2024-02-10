using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorChangeVerifications;

public class GetTutorChangeVerificationsQueryHandler(
    ITutorRepository tutorRepository,
    IUserRepository userRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IUnitOfWork unitOfWork,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTutorChangeVerificationsQuery, TutorVerificationInfoForEditDto>(unitOfWork, logger,
        mapper)
{
    public override async Task<Result<TutorVerificationInfoForEditDto>> Handle(
        GetTutorChangeVerificationsQuery request, CancellationToken cancellationToken)
    {
        var tutorQ =
            from tutorR in tutorRepository.GetAll()
            join userR in userRepository.GetAll() on tutorR.Id equals userR.Id 
            where tutorR.Id == IdentityGuid.Create(request.TutorId)
            select new
            {
                Id = userR.Id.Value,
                Name = userR.FirstName + " " + userR.LastName,
                ChangeVerificationRequestDto = tutorR.ChangeVerificationRequests,
                Currents = tutorR.TutorVerificationInfos.Select(x => x.Image)
            };

        var tutor = await asyncQueryableExecutor.FirstOrDefaultAsync(tutorQ, false, cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorAppServiceError.NonExistTutorError);
        }

        var changeVerificationRequestDtos =
            mapper.Map<List<ChangeVerificationRequestDto>>(tutor.ChangeVerificationRequestDto);

        return new TutorVerificationInfoForEditDto
        {
            ChangeVerificationRequestDtos = changeVerificationRequestDtos,
            TutorVerificationInfoDtos = tutor.Currents,
            TutorName = tutor.Name,
            TutorId = tutor.Id
        };
    }
}