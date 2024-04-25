namespace Matt.ResultObject;

internal interface IResult
{
    bool IsSuccess { get; }

    bool IsFailure { get; }

    string DisplayMessage { get; }
}