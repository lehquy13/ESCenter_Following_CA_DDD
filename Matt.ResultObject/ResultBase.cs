namespace Matt.ResultObject;

public abstract class ResultBase : IResult
{
    private string _displayMessage = string.Empty;

    public bool IsSuccess { get; private init; }

    public bool IsFailure => !IsSuccess;

    public string DisplayMessage
    {
        get => IsSuccess
            ? _displayMessage
            : $"Code: {Error.Code} - {Error.Description}";
        protected init => _displayMessage = value;
    }

    public Error Error { get; }

    protected internal ResultBase(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new ArgumentException("Cannot supply error for successful result");
            case false when error == Error.None:
                throw new ArgumentException("Must supply error for failed result");
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }
}