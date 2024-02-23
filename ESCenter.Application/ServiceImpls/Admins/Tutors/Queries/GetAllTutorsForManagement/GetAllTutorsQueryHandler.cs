using ESCenter.Application.Contracts.Users.Tutors;
using ESCenter.Application.ServiceImpls.Admins.Users.Queries.GetLearners;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetAllTutorsForManagement;

public class GetAllTutorsQueryHandler(
    ITutorRepository tutorRepository,
    IUserRepository userRepository,
    ITutorRequestRepository tutorRequestRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetLearnersQueryHandler> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetAllTutorsQuery, IEnumerable<TutorListDto>>(unitOfWork, logger, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<Result<IEnumerable<TutorListDto>>> Handle(GetAllTutorsQuery request,
        CancellationToken cancellationToken)
    {
        var tutorQ =
            from userR in userRepository.GetAll()
            join tutorR in tutorRepository.GetAll() on userR.Id equals tutorR.UserId
            join tutorRequestR in tutorRequestRepository.GetAll() on tutorR.Id equals tutorRequestR.TutorId into
                tutorRequests
            select new TutorListDto()
            {
                Id = userR.Id.Value,
                FirstName = userR.FirstName,
                LastName = userR.LastName,
                PhoneNumber = userR.PhoneNumber,
                University = tutorR.University,
                IsVerified = tutorR.IsVerified,
                NumberOfChangeRequests = tutorR.ChangeVerificationRequest == null ? 0 : 1,
                NumberOfRequests = tutorRequests.Count()
            };

        var tutors = await asyncQueryableExecutor.ToListAsync(tutorQ, false, cancellationToken);

        return tutors;
    }
}