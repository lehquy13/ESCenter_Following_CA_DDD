using ESCenter.Application.Contract.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Application.ServiceImpls.Clients.Courses.Commands.CreateCourse;

public record CreateCourseByLearnerCommand(CourseForLearnerCreateDto CourseForLearnerCreateDto) : ICommandRequest;