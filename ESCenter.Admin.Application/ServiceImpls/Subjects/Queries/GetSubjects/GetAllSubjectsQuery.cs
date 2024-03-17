using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Subjects.Queries.GetSubjects;

public record GetAllSubjectsQuery() : IQueryRequest<List<SubjectDto>>;
