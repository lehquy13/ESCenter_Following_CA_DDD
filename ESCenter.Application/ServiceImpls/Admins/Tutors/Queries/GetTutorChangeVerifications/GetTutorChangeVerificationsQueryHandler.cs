using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
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
    IAppLogger<GetTutorChangeVerificationsQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTutorChangeVerificationsQuery, VerificationEditDto>(unitOfWork, logger, mapper)
{
    public override async Task<Result<VerificationEditDto>> Handle(
        GetTutorChangeVerificationsQuery request, CancellationToken cancellationToken)
    {
        var tutorQ =
            from tutorR in tutorRepository.GetAll()
            join userR in userRepository.GetAll() on tutorR.UserId equals userR.Id
            where tutorR.Id == TutorId.Create(request.TutorId)
            select new
            {
                Id = userR.Id.Value,
                Name = userR.FirstName + " " + userR.LastName,
                ChangeVerificationRequestDto = tutorR.ChangeVerificationRequest,
                Currents = tutorR.Verifications.Select(x => x.Image)
            };

        var tutor = await asyncQueryableExecutor.FirstOrDefaultAsync(tutorQ, false, cancellationToken);

        if (tutor is null)
        {
            return Result.Fail(TutorAppServiceError.NonExistTutorError);
        }

        var changeVerificationRequestDtos =
            Mapper.Map<List<ChangeVerificationRequestDto>>(tutor.ChangeVerificationRequestDto);

        return new VerificationEditDto
        {
            ChangeVerificationRequestDtos = changeVerificationRequestDtos,
            VerificationDtos = tutor.Currents,
            TutorName = tutor.Name,
            TutorId = tutor.Id
        };
    }
}