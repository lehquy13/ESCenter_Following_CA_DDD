using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices.Interfaces;

public interface IPaymentDomainService : IDomainService
{
    Task<Result> CreatePayment(CourseId courseId);
    Task<Result> CancelPayment();
}