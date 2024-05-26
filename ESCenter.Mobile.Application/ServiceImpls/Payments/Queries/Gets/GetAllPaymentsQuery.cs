using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Mobile.Application.ServiceImpls.Payments.Queries.Gets;

public class PaymentDto
{
    public Guid PaymentId { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public string Code { get; set; } = null!;
}

public record GetAllPaymentsQuery() : IQueryRequest<IEnumerable<PaymentDto>>;

public class GetAllPaymentsQueryHandler(
    IRepository<Payment, PaymentId> paymentRepository,
    ITutorRepository customerRepository,
    ICourseRepository courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    ICurrentUserService currentUserService,
    IAppLogger<GetAllPaymentsQueryHandler> logger,
    IMapper mapper
) : QueryHandlerBase<GetAllPaymentsQuery, IEnumerable<PaymentDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<PaymentDto>>> Handle(GetAllPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var tutorByUserId =
            await customerRepository.GetTutorByUserId(CustomerId.Create(currentUserService.UserId), cancellationToken);

        if (tutorByUserId is null)
        {
            return Result.Fail("Tutor Not Found");
        }

        var paymentQueryable = paymentRepository.GetAll()
            .Where(x => x.TutorId == tutorByUserId.Id)
            .Join(courseRepository.GetAll(),
                payment => payment.CourseId,
                course => course.Id,
                (payment, course) => new PaymentDto()
                {
                    PaymentId = payment.Id.Value,
                    CourseId = payment.CourseId.Value,
                    CourseTitle = course.Title,
                    PaymentStatus = payment.PaymentStatus.ToString(),
                    Code = payment.Code
                });

        var payments = await asyncQueryableExecutor
            .ToListAsSplitAsync(paymentQueryable, false, cancellationToken);

        return payments;
    }
}