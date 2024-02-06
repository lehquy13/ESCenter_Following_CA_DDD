using ESCenter.Application.Contract.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Clients.Courses.Commands.ReviewCourse;

public record ReviewCourseCommand(ReviewDetailDto ReviewDetailDto) : ICommandRequest;