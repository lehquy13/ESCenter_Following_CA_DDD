using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.PurchaseCourse;

public record PurchaseCourseCommand(Guid CourseId) : ICommandRequest, IAuthorizationRequest;