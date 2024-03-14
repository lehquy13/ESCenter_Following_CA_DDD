using ESCenter.Client.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Client.Application.ServiceImpls.Subjects.Queries.GetSubject;

public record GetSubjectQuery(int Id) : IQueryRequest<SubjectDto>;
