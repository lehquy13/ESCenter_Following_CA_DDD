using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetTutoringRequests;

public record GetTutorRequestsQuery : IQueryRequest<IEnumerable<TutorRequestForListDto>>, IAuthorizationRequest;

public class GetTutorRequestsQueryHandler(
    IAppLogger<RequestHandlerBase> logger,
    ICurrentUserService currentUserService,
    ITutorRepository tutorRepository,
    ICustomerRepository customerRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ITutorRequestRepository tutorRequestRepository,
    IMapper mapper
) : QueryHandlerBase<GetTutorRequestsQuery, IEnumerable<TutorRequestForListDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<TutorRequestForListDto>>> Handle(GetTutorRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = CustomerId.Create(currentUserService.UserId);
        
        var queryable = tutorRequestRepository.GetAll()
            .Where(t => t.LearnerId == userId)
            .Join(tutorRepository.GetAll(),
                req => req.TutorId,
                tutor => tutor.Id,
                (req, tutor) => new { req, tutor })
            .Join(customerRepository.GetAll(),
                t => t.tutor.CustomerId,
                customer => customer.Id,
                (t, customer) => new { t.req, customer })
            .OrderByDescending(t => t.req.RequestStatus)
            .Select(t => new TutorRequestForListDto
            {
                Id = t.req.Id.Value,
                RequestMessage = t.req.Message,
                Status = t.req.RequestStatus,
                TutorFullName = t.customer.FirstName + t.customer.LastName,
                CreationTime = t.req.CreationTime
            });

        return await asyncQueryableExecutor.ToListAsync(queryable, false, cancellationToken);
    }
}

public class TutorRequestForListDto
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public string TutorFullName {get; set; } = null!;
    public string RequestMessage { get; set; } = null!;
    public RequestStatus Status { get; set; }
}