using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Tutors;

public class TutorBySubjectIdSpec : GetListSpecificationBase<Tutor>
{
    public TutorBySubjectIdSpec(SubjectId subjectId)
    {
        Criteria = criteria
            => criteria.TutorMajors.Any(x => x.SubjectId == subjectId);
    }
}