using ESCenter.Domain.Aggregates.Tutors.DomainEvents;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Tutors;

public class Tutor : AuditedAggregateRoot<TutorId>
{
    private List<Verification> _verifications = new();
    private List<TutorMajor> _tutorMajors = new();

    public AcademicLevel AcademicLevel { get; private set; } = AcademicLevel.UnderGraduated;
    public string University { get; private set; } = string.Empty;
    public bool IsVerified { get; private set; }
    public float Rate { get; private set; }
    public CustomerId CustomerId { get; private set; } = null!;
    public IReadOnlyList<Verification> Verifications => _verifications.AsReadOnly();
    public IReadOnlyList<TutorMajor> TutorMajors => _tutorMajors.AsReadOnly();
    public ChangeVerificationRequest? ChangeVerificationRequest { get; private set; }

    public void CreateChangeVerificationRequest(List<string> urls)
    {
        ChangeVerificationRequest = ChangeVerificationRequest.Create(Id, urls);
        IsVerified = false;

        DomainEvents.Add(new ChangeVerificationRequestCreatedDomainEvent(this,
            ChangeVerificationRequest.ChangeVerificationRequestDetails.Count));
    }

    public void AddTutorMajor(TutorMajor tutorMajor)
    {
        _tutorMajors.Add(tutorMajor);
    }

    public void UpdateAllMajor(List<TutorMajor> tutorMajors)
    {
        _tutorMajors = tutorMajors;
    }

    private Tutor()
    {
    }

    public static Tutor Create(
        CustomerId userId,
        AcademicLevel academicLevel,
        string university,
        IEnumerable<string> verificationInfos,
        bool isVerified,
        short rate = 0)
    {
        var id = TutorId.Create();

        var verifications = verificationInfos.Select(i => Verification.Create(i, id)).ToList();

        return new Tutor
        {
            Id = id,
            CustomerId = userId,
            AcademicLevel = academicLevel,
            _verifications = verifications,
            University = university,
            IsVerified = isVerified,
            Rate = rate,
        };
    }

    public void Verify(bool isVerified)
    {
        IsVerified = isVerified;
    }

    public void UpdateRate(float reviewRate)
    {
        Rate = reviewRate;
    }

    public void RemoveMajor(TutorMajor major)
    {
        _tutorMajors.Remove(major);
    }

    public void SetUserId(CustomerId id)
    {
        CustomerId = id;
    }

    public Result ModifyChangeVerificationRequest(bool commandIsApproved)
    {
        if (ChangeVerificationRequest is null)
        {
            return Result.Fail("There is no change verification request");
        }

        if (commandIsApproved is false)
        {
            ChangeVerificationRequest.Reject();
        }
        else
        {
            ChangeVerificationRequest.Approve();

            var verificationInfos = ChangeVerificationRequest.ChangeVerificationRequestDetails
                .Select(id => Verification.Create(id.ImageUrl, Id))
                .ToList();

            _verifications = verificationInfos;
        }

        ChangeVerificationRequest = null;

        return Result.Success();
    }
}