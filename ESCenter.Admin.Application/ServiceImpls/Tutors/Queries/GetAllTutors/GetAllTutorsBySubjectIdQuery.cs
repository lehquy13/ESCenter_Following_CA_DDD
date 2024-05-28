using ESCenter.Admin.Application.Contracts.Users.BasicUsers;
using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetAllTutors;

public record GetAllTutorsBySubjectIdQuery(int SubjectId) : IQueryRequest<List<UserForListDto>>;