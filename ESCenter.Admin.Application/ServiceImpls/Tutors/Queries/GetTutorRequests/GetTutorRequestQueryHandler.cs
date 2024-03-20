using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorRequests;

public class GetTutorRequestQueryHandler(
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ITutorRequestRepository tutorRequestRepository,
    ITutorRepository tutorRepository,
    ICustomerRepository customerRepository,
    IAppLogger<GetTutorRequestQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTutorRequestQuery, List<TutorRequestForListDto>>(logger, mapper)
{
    public override async Task<Result<List<TutorRequestForListDto>>> Handle(GetTutorRequestQuery request,
        CancellationToken cancellationToken)
    {
        var tutorId = CustomerId.Create(request.TutorId);

        var queryable =
            from req in tutorRequestRepository.GetAll()
            join tutor in tutorRepository.GetAll() on req.TutorId equals tutor.Id
            join user in customerRepository.GetAll() on req.LearnerId equals user.Id
            where req.TutorId == tutorId
            select new
            {
                Tutor = tutor,
                Learner = user,
                TutorRequest = req.Message
            };

        var result = await asyncQueryableExecutor.ToListAsync(queryable, false, cancellationToken);

        var resultDto = Mapper.Map<List<TutorRequestForListDto>>(result);
        return resultDto;
    }
}