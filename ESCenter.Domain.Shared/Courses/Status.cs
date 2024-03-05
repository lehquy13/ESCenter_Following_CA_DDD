namespace ESCenter.Domain.Shared.Courses;

public enum Status
{
    Waiting = 0,
    Confirmed = 1,
    Canceled = 2,
    OnConfirming = 3,
    Available = 4,
    OnPurchasing = 5,
    OnVerifying = 6,
    None = 7
}