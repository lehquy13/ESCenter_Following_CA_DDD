using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Courses.Commands.RemoveReview;

public record RemoveCourseReviewCommand(Guid CourseId) : ICommandRequest;