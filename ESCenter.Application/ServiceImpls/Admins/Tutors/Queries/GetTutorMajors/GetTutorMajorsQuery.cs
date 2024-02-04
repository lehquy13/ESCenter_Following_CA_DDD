using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Application.ServiceImpls.Admins.Tutors.Queries.GetTutorMajors;

public record GetTutorMajorsQuery(Guid TutorId) : IQueryRequest<List<TutorMajorDto>>;

public class TutorMajorDto
{
    public Guid TutorId { get; private set; }
    public int SubjectId { get; private set; }
    public string SubjectName { get; private set; } = null!;
}