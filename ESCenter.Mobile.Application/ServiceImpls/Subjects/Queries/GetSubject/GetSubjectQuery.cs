using ESCenter.Mobile.Application.Contracts.Courses.Dtos;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Mobile.Application.ServiceImpls.Subjects.Queries.GetSubject;

public record GetSubjectQuery(int Id) : IQueryRequest<SubjectDto>;
