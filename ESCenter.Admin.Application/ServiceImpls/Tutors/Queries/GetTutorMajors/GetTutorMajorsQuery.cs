using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetTutorMajors;

public record GetTutorMajorsQuery(Guid TutorId) : IQueryRequest<List<SubjectDto>>;