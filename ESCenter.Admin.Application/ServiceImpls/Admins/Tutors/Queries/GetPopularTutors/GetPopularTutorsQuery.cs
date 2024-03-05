using ESCenter.Domain.Aggregates.Tutors;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Admin.Application.ServiceImpls.Admins.Tutors.Queries.GetPopularTutors;

public record GetPopularTutorsQuery() : IQueryRequest<List<PopularTutorListDto>>;

public class GetPopularTutorsQueryHandler(
    ITutorRepository tutorRepository,
    IAppLogger<GetPopularTutorsQueryHandler> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : QueryHandlerBase<GetPopularTutorsQuery, List<PopularTutorListDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<List<PopularTutorListDto>>> Handle(GetPopularTutorsQuery request, CancellationToken cancellationToken)
    {
        var popularTutorsAsQueryable = await tutorRepository.GetPopularTutors();
        
        var popularTutorListDtos = Mapper.Map<List<PopularTutorListDto>>(popularTutorsAsQueryable);

        return popularTutorListDtos;
    }
}

public class PopularTutorListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string University { get; set; } = null!;
    public string AcademicLevel { get; set; } = null!;
    public string Description { get; set; } = null!;
}