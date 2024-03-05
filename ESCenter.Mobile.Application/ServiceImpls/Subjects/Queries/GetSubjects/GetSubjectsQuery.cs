using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Subjects.Queries.GetSubjects;

public record GetSubjectsQuery() : IQueryRequest<List<SubjectDto>>;
