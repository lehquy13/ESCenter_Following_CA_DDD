using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.PurchaseCourse;

/// <summary>
/// Deprecated
/// </summary>
/// <param name="CourseId"></param>
public record PurchaseCourseCommand(Guid CourseId) : ICommandRequest, IAuthorizationRequest;