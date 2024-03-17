using Matt.SharedKernel.Application.Mediators.Queries;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Queries.GetPopularTutors;

public record GetPopularTutorsQuery() : IQueryRequest<List<PopularTutorListDto>>;

public class PopularTutorListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string University { get; set; } = null!;
    public string AcademicLevel { get; set; } = null!;
    public string Description { get; set; } = null!;
}