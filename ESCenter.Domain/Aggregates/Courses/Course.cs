using ESCenter.Domain.Aggregates.Courses.CourseRequests;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.ResultObject;
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

    public string LearnerGender { get; private set; } = Gender.Male.ToString();
    public string LearnerName { get; private set; } = string.Empty;
    public int NumberOfLearner { get; private set; } = 1;
    public string ContactNumber { get; private set; } = string.Empty;
    public IdentityGuid? LearnerId { get; private set; }

    public IReadOnlyCollection<CourseRequest> CourseRequests => _courseRequests.AsReadOnly();

    public IdentityGuid? TutorId { get; private set; }
    public Review? Review { get; private set; }

    private Course()
    {
    }

    private Course(
        string title,
        string description,
        LearningMode learningMode,
        Fee sectionFee,
        Fee chargeFee,
        Gender genderRequirement,
        AcademicLevel academicLevelRequirement,
        string learnerGender,
        string learnerName,
        int numberOfLearner,
        string contactNumber,
        SessionDuration minutePerSession,
        SessionPerWeek sessionPerWeek,
        string address,
        SubjectId subjectId,
        IdentityGuid? learnerId)
    {
        Title = title;
        Description = description;
        LearningMode = learningMode;
        SectionFee = sectionFee;
        ChargeFee = chargeFee;
        GenderRequirement = genderRequirement;
        AcademicLevelRequirement = academicLevelRequirement;
        LearnerGender = learnerGender;
        LearnerName = learnerName;
        NumberOfLearner = numberOfLearner;
        ContactNumber = contactNumber;
        SessionDuration = minutePerSession;
        SessionPerWeek = sessionPerWeek;
        Address = address;
        SubjectId = subjectId;
        LearnerId = learnerId;
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
        string learnerGender,
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
        return new Course(
            title,
            description,
            learningMode,
            Fee.Create(sectionFee, currency),
            Fee.Create(chargeFee, currency),
            genderRequirement,
            academicLevelRequirement,
            learnerGender,
            learnerName,
            numberOfLearner,
            contactNumber,
            SessionDuration.Create(sectionDuration, sectionType),
            SessionPerWeek.Create(sectionPerWeek),
            address,
            subjectId,
            learnerId
        );
    }

    public void ReviewCourse(short rate, string detail)
    {
        if (Status != Status.Confirmed)
        {
            throw new Exception("Course is not on going");
        }

        Review = Review.Create(rate, detail, Id);
    }

    public void SetLearner(string learnerName, string learnerGender, string contactNumber)
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
}