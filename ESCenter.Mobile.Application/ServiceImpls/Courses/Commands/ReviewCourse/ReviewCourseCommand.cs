using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Mobile.Application.ServiceImpls.Courses.Commands.ReviewCourse;

public record ReviewCourseCommand(ReviewDetailDto ReviewDetailDto) : ICommandRequest;