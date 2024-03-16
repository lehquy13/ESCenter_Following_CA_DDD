using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using MapsterMapper;
using Matt.ResultObject;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Application.Mediators;
using Matt.SharedKernel.Application.Mediators.Queries;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;

namespace ESCenter.Client.Application.ServiceImpls.Profiles.Queries.GetTutorMajors;

public class GetTutorMajorsQueryHandler(
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ISubjectRepository subjectRepository,
    IReadOnlyRepository<TutorMajor, TutorMajorId> tutorMajorRepository,
    IAsyncQueryableExecutor asyncQueryableExecutor,
    IAppLogger<RequestHandlerBase> logger,
    IMapper mapper) : QueryHandlerBase<GetTutorMajorsQuery, IEnumerable<SubjectMajorDto>>(unitOfWork, logger, mapper)
{
    public override async Task<Result<IEnumerable<SubjectMajorDto>>> Handle(GetTutorMajorsQuery request,
        CancellationToken cancellationToken)
    {
        var tutorQ =
            subjectRepository.GetAll()
                .GroupJoin(
                    tutorMajorRepository
                        .GetAll()
                        .Where(x => x.TutorId == TutorId.Create(currentUserService.UserId)),
                    subject => subject.Id,
                    tutor => tutor.SubjectId,
                    (subject, tutor) => new SubjectMajorDto
                    {
                        Id = subject.Id.Value,
                        Name = subject.Name,
                        IsSelected = tutor.Any()
                    });

        // filter out the subjects that the tutor is not majoring in
        var tutorMajors = await asyncQueryableExecutor.ToListAsync(tutorQ, false, cancellationToken);
        
        return tutorMajors;
    }
}