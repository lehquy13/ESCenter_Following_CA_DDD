namespace Matt.ResultObject;

public class Result : ResultBase
{
    private Result(bool isSuccess, Error error)
        : base(isSuccess, error)
    {
    }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Success(string message)
    {
        return new Result(true, Error.None) { DisplayMessage = message };
    }

    public static Result Fail(Error error)
    {
        return new Result(false, error);
    }

    public static Result Fail(string errorMessage)
    {
        return new Result(false, new Error("Unexpected error", errorMessage));
    }

    public static Error Conflict(
        string code = "General.Conflict",
        string description = "A conflict error has occurred.") =>
        new(code, description);

    public static Error NotFound(
        string code = "General.NotFound",
        string description = "A 'Not Found' error has occurred.") =>
        new(code, description);
    
    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "An 'Unauthorized' error has occurred.") =>
        new(code, description);
    
    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "A 'Forbidden' error has occurred.") =>
        new(code, description);

    #region implicit operators

    /// <summary>
    /// Implicitly convert from Error to Result
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static implicit operator Result(Error error)
    {
        return new Result(false, error);
    }

    /// <summary>
    /// Implicitly convert from Exception to Result
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static implicit operator Result(Exception error)
    {
        return new Result(false, new Error("Unexpected error with exception", error.Message));
    }

    #endregion
}