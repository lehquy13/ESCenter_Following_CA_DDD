using ESCenter.Client.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubjects;

public record GetSubjectsQuery() : IQueryRequest<List<SubjectDto>>;
