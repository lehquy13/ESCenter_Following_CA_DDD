using ESCenter.Domain.Aggregates.Subjects;
using Matt.SharedKernel.Domain.Specifications;

namespace ESCenter.Domain.Specifications.Subjects;

public class SubjectListByNameSpec : GetListSpecificationBase<Subject>
{
    public SubjectListByNameSpec(List<string> subjectNames)
    {
        Criteria = subject => subjectNames.Contains(subject.Name);
    }
}