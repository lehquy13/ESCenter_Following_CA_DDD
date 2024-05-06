using ESCenter.Admin.Application.Contracts.Users.Learners;
using ESCenter.Admin.Application.ServiceImpls.Tutors;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.TutorRequests.Queries.GetTutorRequestsByTutorId;

public class GetTutorRequestsByTutorIdQueryHandler(
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ITutorRequestRepository tutorRequestRepository,
    ICustomerRepository customerRepository,
    IAppLogger<GetTutorRequestsByTutorIdQueryHandler> logger,
    IMapper mapper)
    : QueryHandlerBase<GetTutorRequestsByTutorIdQuery, List<TutorRequestForListDto>>(logger, mapper)
{
    public override async Task<Result<List<TutorRequestForListDto>>> Handle(GetTutorRequestsByTutorIdQuery requestsByTutorId,
        CancellationToken cancellationToken)
    {
        var tutorId = TutorId.Create(requestsByTutorId.TutorId);

        var queryable =
            from req in tutorRequestRepository.GetAll()
            join user in customerRepository.GetAll() on req.LearnerId equals user.Id
            where req.TutorId == tutorId
            select new TutorRequestForListDto
            {
                Id = req.Id.Value,
                TutorId = req.TutorId.Value,
                LearnerId = req.LearnerId.Value,
                PhoneNumber = user.PhoneNumber,
                Name = user.FirstName + " " + user.LastName,
                RequestMessage = req.Message
            };

        var results = await asyncQueryableExecutor.ToListAsync(queryable, false, cancellationToken);

        foreach (var tutorRequestForListDto in results)
        {
            {
                var tutor = await customerRepository.GetTutorByTutorId(TutorId.Create(tutorRequestForListDto.TutorId),
                    cancellationToken);

                if (tutor == null)
                {
                    return Result.Fail(TutorAppServiceError.NonExistTutorOfCreatedRequestError);
                }

                tutorRequestForListDto.TutorEmail = tutor.Email;
                tutorRequestForListDto.TutorFullName = tutor.FirstName + " " + tutor.LastName;
                tutorRequestForListDto.TutorPhoneNumber = tutor.PhoneNumber;
            }
        }

        return results;
    }
}