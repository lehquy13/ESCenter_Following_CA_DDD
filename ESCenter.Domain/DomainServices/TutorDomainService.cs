using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.DomainServices.Interfaces;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Interfaces;

namespace ESCenter.Domain.DomainServices;

public class TutorDomainService(
    IAppLogger<TutorDomainService> logger,
    ITutorRepository tutorRepository,
    ISubjectRepository subjectRepository
)
    : DomainServiceBase(logger), ITutorDomainService
{
    private static readonly List<string> EmptyVerification = ["document.png"];

    public async Task<Tutor> CreateTutorWithEmptyVerificationAsync(
        CustomerId userId,
        AcademicLevel academicLevel,
        string university,
        IEnumerable<int> majors,
        bool isVerified)
    {
        var tutor = Tutor.Create(
            userId,
            academicLevel,
            university,
            EmptyVerification,
            isVerified
        );
        var subjects = await subjectRepository.GetListByIdsAsync(majors.Select(SubjectId.Create));

        // add new majors to tutor
        var tutorMajors =
            subjects
                .Select(x => TutorMajor.Create(tutor.Id, x.Id, x.Name))
                .ToList();

        tutor.UpdateAllMajor(tutorMajors);

        await tutorRepository.InsertAsync(tutor);

        return tutor;
    }
}