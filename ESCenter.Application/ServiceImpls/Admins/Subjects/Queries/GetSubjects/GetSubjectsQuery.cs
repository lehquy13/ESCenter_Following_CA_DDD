using ESCenter.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Subjects.Queries.GetSubjects;

public record GetSubjectsQuery() : IQueryRequest<List<SubjectDto>>;
