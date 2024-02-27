using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Courses;

public sealed class Course : FullAuditedAggregateRoot<CourseId>
{
    private List<CourseRequest> _courseRequests = new();
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Status Status { get; private set; } = Status.OnVerifying;
    public LearningMode LearningMode { get; private set; } = LearningMode.Offline;
    public Fee SectionFee { get; private set; } = Fee.Create(0, Currency.USD);
    public Fee ChargeFee { get; private set; } = Fee.Create(0, Currency.USD);
    public Gender GenderRequirement { get; private set; } = Gender.None;
    public AcademicLevel AcademicLevelRequirement { get; private set; } = AcademicLevel.Optional;
    public SessionDuration SessionDuration { get; private set; } = SessionDuration.Create();
    public SessionPerWeek SessionPerWeek { get; private set; } = SessionPerWeek.Create(1);
    public string Address { get; private set; } = string.Empty; // TODO: Change this one to Address Value Object
    public SubjectId SubjectId { get; private set; } = null!;

    public Gender LearnerGender { get; private set; } = Gender.Male;
    public string LearnerName { get; private set; } = string.Empty;
    public int NumberOfLearner { get; private set; } = 1;
    public string ContactNumber { get; private set; } = string.Empty;
    public IdentityGuid? LearnerId { get; private set; }

    public IReadOnlyCollection<CourseRequest> CourseRequests => _courseRequests.AsReadOnly();

    public TutorId? TutorId { get; private set; }
    public Review? Review { get; private set; }

    private Course()
    {
    }

    public static Course Create(
        string title,
        string description,
        LearningMode learningMode,
        float sectionFee,
        float chargeFee,
        string? currency,
        Gender genderRequirement,
        AcademicLevel academicLevelRequirement,
        Gender learnerGender,
        string learnerName,
        int numberOfLearner,
        string contactNumber,
        int sectionDuration,
        string? sectionType,
        int sectionPerWeek,
        string address,
        SubjectId subjectId,
        IdentityGuid? learnerId)
    {
        return new Course()
        {
            Id = CourseId.Create(),
            Title = title,
            Description = description,
            LearningMode = learningMode,
            SectionFee = Fee.Create(sectionFee, currency),
            ChargeFee = Fee.Create(chargeFee, currency),
            GenderRequirement = genderRequirement,
            AcademicLevelRequirement = academicLevelRequirement,
            LearnerGender = learnerGender,
            LearnerName = learnerName,
            NumberOfLearner = numberOfLearner,
            ContactNumber = contactNumber,
            SessionDuration = SessionDuration.Create(sectionDuration, sectionType),
            SessionPerWeek = SessionPerWeek.Create(sectionPerWeek),
            Address = address,
            SubjectId = subjectId,
            LearnerId = learnerId
        };
    }

    public void ReviewCourse(short rate, string detail)
    {
        if (Status != Status.Confirmed)
        {
            throw new Exception("Course is not on going");
        }

        Review = Review.Create(rate, detail, Id);
    }

    public void SetLearner(string learnerName, Gender learnerGender, string contactNumber)
    {
        LearnerName = learnerName;
        LearnerGender = learnerGender;
        ContactNumber = contactNumber;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }

    public void RemoveReview()
    {
        Review = null;
    }

    public void Request(CourseRequest courseRequestToCreate)
    {
        if (Status != Status.Confirmed)
        {
            throw new Exception("Course is not on going");
        }

        if (_courseRequests.Any(x => x.TutorId == courseRequestToCreate.TutorId))
        {
            throw new Exception("Requested course error");
        }

        _courseRequests.Add(courseRequestToCreate);
    }

    public void SetLearnerId(IdentityGuid learnerId)
    {
        LearnerId = learnerId;
    }

    public void AssignTutor(TutorId tutorId)
    {
        TutorId = tutorId;
        Status = Status.Confirmed;
    }

    public void UpdateCourse(string title,
        string description,
        LearningMode learningMode,
        float sectionFee,
        float chargeFee,
        Gender genderRequirement,
        AcademicLevel academicLevelRequirement,
        Gender learnerGender,
        string learnerName,
        int numberOfLearner,
        string contactNumber,
        int sectionDuration,
        int sectionPerWeek,
        string address,
        Status status,
        SubjectId subjectId)
    {
        Title = title;
        Description = description;
        LearningMode = learningMode;
        SectionFee = Fee.Create(sectionFee, Currency.USD);
        ChargeFee = Fee.Create(chargeFee, Currency.USD);
        GenderRequirement = genderRequirement;
        AcademicLevelRequirement = academicLevelRequirement;
        LearnerGender = learnerGender;
        LearnerName = learnerName;
        NumberOfLearner = numberOfLearner;
        ContactNumber = contactNumber;
        SessionDuration = SessionDuration.Create(sectionDuration);
        SessionPerWeek = SessionPerWeek.Create(sectionPerWeek);
        Address = address;
        SubjectId = subjectId;
        Status = status;
    }

    public void UnAssignTutor()
    {
        TutorId = null;
        Status = Status.OnVerifying;
    }
}