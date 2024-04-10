namespace ESCenter.Domain.Shared.Courses;

public enum Status
{
    OnVerifying = 0,
    Confirmed = 1,
    Canceled = 2,
    OnPurchasing = 3,
    Available = 4,
    OnProgressing = 5,
    None = 7
}