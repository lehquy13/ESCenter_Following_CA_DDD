using ESCenter.Domain.Aggregates.Courses.DomainEvents;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Courses.Errors;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Aggregates.Users.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Domain.Shared.NotificationConsts;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Courses;

public sealed class Course : FullAuditedAggregateRoot<CourseId>
{
    private List<CourseRequest> _courseRequests = new();
    private const int MaxCourseRequests = 5;

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
    public CustomerId? LearnerId { get; private set; }

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
        CustomerId? learnerId)
    {
        return new Course
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

    public Result ReviewCourse(short rate, string detail)
    {
        if (Status != Status.Confirmed)
        {
            return Result.Fail(CourseDomainError.CourseIsNotOnGoing);
        }

        var result = Review.Create(rate, detail, Id);

        if (!result.IsSuccess)
        {
            return result.Error;
        }

        Review = result.Value;

        DomainEvents.Add(new CourseReviewedDomainEvent(this));
        DomainEvents.Add(new NewDomainObjectCreatedEvent(
            Id.Value.ToString(),
            $"Review class: {Title} at {CreationTime.ToLongDateString()}",
            NotificationEnum.Course));

        return Result.Success();
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

    public Result RemoveReview()
    {
        Review = null;
        DomainEvents.Add(new CourseReviewedDomainEvent(this));
        DomainEvents.Add(new NewDomainObjectCreatedEvent(
            Id.Value.ToString(),
            $"Review class: {Title} at {CreationTime.ToLongDateString()}",
            NotificationEnum.Course));

        return Result.Success();
    }

    public Result Request(CourseRequest courseRequestToCreate)
    {
        if (Status != Status.Confirmed)
        {
            return Result.Fail(CourseDomainError.CourseIsNotOnGoing);
        }

        if (_courseRequests.Any(x => x.TutorId == courseRequestToCreate.TutorId))
        {
            return Result.Fail(CourseDomainError.RequestedCourseError);
        }

        if (_courseRequests.Count >= MaxCourseRequests)
        {
            return Result.Fail(CourseDomainError.MaxCourseRequestError);
        }

        _courseRequests.Add(courseRequestToCreate);

        if (_courseRequests.Count == MaxCourseRequests)
        {
            Status = Status.OnProgressing;
        }

        return Result.Success();
    }

    public void SetLearnerId(CustomerId learnerId)
    {
        LearnerId = learnerId;
    }

    public void AssignTutor(TutorId tutorId)
    {
        TutorId = tutorId;
        Status = Status.Confirmed;

        // Cancel all other course requests
        foreach (var courseRequest in _courseRequests)
        {
            if (courseRequest.TutorId == tutorId)
            {
                courseRequest.Approved();
                continue;
            }

            courseRequest.Cancel();
        }

        DomainEvents.Add(new CourseAssignedDomainEvent(this, tutorId));
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

    public Result Purchase(TutorId tutorId)
    {
        if(Status != Status.Confirmed || Status == Status.Canceled || Status == Status.OnVerifying)
        {
            return Result.Fail(CourseDomainError.CourseUnavailable); 
        }

        Status = Status.OnProgressing;

        DomainEvents.Add(new CoursePurchasedDomainEvent(this, tutorId));
        
        return Result.Success();
    }
}