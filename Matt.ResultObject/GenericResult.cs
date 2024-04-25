namespace Matt.ResultObject;

public class Result<T> : ResultBase, IHasErrorDetail where T : notnull
{
    public T Value { get; }

    private Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        Value = value;
    }

    /// <summary>
    /// Static factory method to create a successful result.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, Error.None);
    }

    /// <summary>
    /// Static factory method to create a successful result with a message.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Result<T> Success(T value, string message)
    {
        return new Result<T>(value, true, Error.None) { DisplayMessage = message };
    }

    /// <summary>
    /// Static factory method to create a failed result with an error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result<T> Fail(Error error)
    {
        return Result.Fail(error);
    }

    /// <summary>
    /// Static factory method to create a failed result with an error.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result<T> Fail(T value, Error error)
    {
        return new Result<T>(value, false, error);
    }

    /// <summary>
    /// Static factory method to create a failed result with an error message.
    /// It calls Result.Fail(string errorMessage) method.
    /// Then the result is converted to Result<![CDATA[T]]> >implicitly.
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static Result<T> Fail(string errorMessage)
    {
        return Result.Fail(errorMessage);
    }

    #region implicit operators

    /// <summary>
    /// A result object can be implicitly converted to Result<![CDATA[T]]>.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(Result result)
    {
        return new Result<T>(default!, result.IsSuccess, result.Error);
    }

    /// <summary>
    /// Implicitly convert an exception to a failed result.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(Exception error)
    {
        return Result.Fail(
            new Error("Unexpected error with exception", error.Message)
        );
    }

    /// <summary>
    /// Implicitly convert a value to its possible result.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value, true, Error.None);
    }

    /// <summary>
    /// Implicitly convert an error to a failed result.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(Error error)
    {
        return Result.Fail(error);
    }

    #endregion
}