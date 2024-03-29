using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Subjects;

public class SubjectListByIdSpec : GetListSpecificationBase<Subject>
{
    public SubjectListByIdSpec(ICollection<int> subjectIds)
    {
        var readSubjectIds = subjectIds.Select(SubjectId.Create).ToList();
        Criteria = subject => readSubjectIds.Contains(subject.Id);
    }
}