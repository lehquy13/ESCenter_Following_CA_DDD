namespace ESCenter.Domain.Shared.Courses;

public enum Status
{
    PendingApproval = 0,
    Confirmed = 1,
    Canceled = 2,
    OnProgressing = 3,
    Available = 4,
    UnverifiedPayment = 5,
    None = 7
}