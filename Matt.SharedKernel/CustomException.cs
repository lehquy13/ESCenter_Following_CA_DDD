namespace Matt.SharedKernel;

public class CustomException(string message) : Exception(message);
public class NotFoundException(string message) : Exception(message);

public class UnauthorizedException(string message) : Exception(message);

public class ValidationException( Dictionary<string, List<string>> errors) : Exception("Validation error occurred.")
{
    public Dictionary<string, List<string>> Errors { get; } = errors;
}