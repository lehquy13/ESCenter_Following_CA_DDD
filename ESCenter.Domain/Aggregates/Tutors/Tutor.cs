using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Tutors;

public class Tutor : AuditedAggregateRoot<TutorId>
{
    private List<TutorVerificationInfo> _tutorVerificationInfos = new();
    private List<ChangeVerificationRequest> _changeVerificationRequests = new();
    private List<TutorMajor> _tutorMajors = new();

    //public IdentityGuid UserId { get; private set; } = null!;
    public AcademicLevel AcademicLevel { get; private set; } = AcademicLevel.Student;
    public string University { get; private set; } = string.Empty;
    public bool IsVerified { get; private set; }
    public float Rate { get; private set; }

    public IdentityGuid UserId { get; private set; } = null!;

    // Acceptable entity because it is an entity that belongs to the aggregate root
    public IReadOnlyList<TutorVerificationInfo> TutorVerificationInfos => _tutorVerificationInfos.AsReadOnly();

    public IReadOnlyList<ChangeVerificationRequest> ChangeVerificationRequests =>
        _changeVerificationRequests.AsReadOnly();

    public IReadOnlyList<TutorMajor> TutorMajors =>
        _tutorMajors.AsReadOnly();

    /// <summary>
    /// Update tutor's information and change the state into being verified
    /// </summary>
    /// <param name="tutor"></param>
    public void UpdateTutorInformation(Tutor tutor)
    {
        AcademicLevel = tutor.AcademicLevel;
        University = tutor.University;

        //wait for being verified
        IsVerified = false;
    }

    public void UpdateTutorVerificationInfo(List<TutorVerificationInfo> tutorVerificationInfos)
    {
        _tutorVerificationInfos = tutorVerificationInfos;
    }

    public void CreateChangeVerificationRequest(ChangeVerificationRequest tutorVerificationInfos)
    {
        _changeVerificationRequests.Add(tutorVerificationInfos);
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
        IdentityGuid userId,
        AcademicLevel academicLevel,
        string university,
        List<string> verificationInfos,
        bool isVerified,
        short rate = 0)
    {
        var id = TutorId.Create();

        var tutorVerificationInfos = new List<TutorVerificationInfo>();
        foreach (var i in verificationInfos)
        {
            var tutorVerificationInfo = TutorVerificationInfo.Create(
                i,
                id
            );

            tutorVerificationInfos.Add(tutorVerificationInfo);
        }


        return new()
        {
            Id = id,
            UserId = userId,
            AcademicLevel = academicLevel,
            _tutorVerificationInfos = tutorVerificationInfos,
            University = university,
            IsVerified = isVerified,
            Rate = rate,
        };
    }

    public void Verify(bool isVerified)
    {
        IsVerified = isVerified;
    }

    public void UpdateRate(double reviewRate)
    {
        Rate = (float)reviewRate;
    }

    public void RemoveMajor(TutorMajor major)
    {
        _tutorMajors.Remove(major);
    }

    public void SetUserId(IdentityGuid id)
    {
        UserId = id;
    }
}