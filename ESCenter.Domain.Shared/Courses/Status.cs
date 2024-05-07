namespace ESCenter.Domain.Shared.Courses;

public enum Status
{
    OnVerifying = 0,
    Confirmed = 1,
    Canceled = 2,
    Available = 4,
    OnProgressing = 3,
    None = 7
}