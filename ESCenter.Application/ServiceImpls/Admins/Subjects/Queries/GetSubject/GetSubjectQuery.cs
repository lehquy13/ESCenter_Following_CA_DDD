using ESCenter.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Subjects.Queries.GetSubject;

public record GetSubjectQuery(int Id) : IQueryRequest<SubjectDto>;
