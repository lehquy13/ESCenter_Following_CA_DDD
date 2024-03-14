using ESCenter.Client.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Client.Application.ServiceImpls.Courses.Commands.CreateCourseRequest;

public record CreateCourseRequestCommand(CourseRequestForCreateDto CourseRequestForCreateDto) : ICommandRequest;