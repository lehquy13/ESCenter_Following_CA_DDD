using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Payment;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Admin.Application.ServiceImpls.Payments.Queries.Gets;

public class GetAllPaymentsQueryHandler(
    IRepository<Payment, PaymentId> paymentRepository,
    ICustomerRepository customerRepository,
    ICourseRepository courseRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<GetAllPaymentsQueryHandler> logger,
    IMapper mapper
) : QueryHandlerBase<GetAllPaymentsQuery, IEnumerable<PaymentDto>>(logger, mapper)
{
    public override async Task<Result<IEnumerable<PaymentDto>>> Handle(GetAllPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var paymentQueryable = paymentRepository.GetAll()
            .OrderByDescending(x => x.LastModificationTime)
            .ThenBy(x => x.CreationTime)
            .Join(courseRepository.GetAll(),
                payment => payment.CourseId,
                course => course.Id,
                (payment, course) => new PaymentDto()
                {
                    PaymentId = payment.Id.Value,
                    TutorId = payment.TutorId.Value,
                    CourseId = payment.CourseId.Value,
                    CourseTitle = course.Title,
                    PaymentStatus = payment.PaymentStatus.ToString(),
                    Code = payment.Code
                });

        var payments = await asyncQueryableExecutor
            .ToListAsync(paymentQueryable, false, cancellationToken);

        foreach (var payment in payments)
        {
            var tutor = await customerRepository
                .GetTutorByTutorId(TutorId.Create(payment.TutorId), cancellationToken);

            if (tutor is null)
            {
                return Result.Fail("Tutor not found");
            }
            
            payment.TutorName = tutor.GetFullName();
            payment.TutorEmail = tutor.Email;
        }

        return payments;
    }
}