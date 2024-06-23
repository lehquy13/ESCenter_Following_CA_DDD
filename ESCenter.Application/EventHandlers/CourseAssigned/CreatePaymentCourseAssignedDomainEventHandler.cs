﻿// using ESCenter.Domain.Aggregates.Courses.DomainEvents;
// using ESCenter.Domain.Aggregates.Payment;
// using ESCenter.Domain.DomainServices.Errors;
// using ESCenter.Domain.Shared.Courses;
// using FCM.Net;
// using Matt.SharedKernel.Domain.EventualConsistency;
// using Matt.SharedKernel.Domain.Interfaces;
// using Matt.SharedKernel.Domain.Interfaces.Repositories;
// using MediatR;
//
// namespace ESCenter.Application.EventHandlers.CourseAssigned;
//
// public class CreatePaymentCourseWhenAssignedDomainEventHandler(
//     IRepository<Payment, PaymentId> paymentRepository,
//     IUnitOfWork unitOfWork
// ) : INotificationHandler<TutorAssignedDomainEvent>
// {
//     public async Task Handle(TutorAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
//     {
//         if (domainEvent.Course.Status == Status.OnProgressing)
//         {
//             var course = domainEvent.Course;
//
//             if (course.Status != Status.OnProgressing || course.TutorId == null)
//             {
//                 throw new EventualConsistencyException(DomainServiceErrors.InvalidCourseStatusForPayment);
//             }
//
//             var payment = Payment.Create(course.TutorId, course.Id, course.ChargeFee.Amount);
//
//             await paymentRepository.InsertAsync(payment, cancellationToken);
//
//             await unitOfWork.SaveChangesAsync(cancellationToken);
//         }
//     }
// }