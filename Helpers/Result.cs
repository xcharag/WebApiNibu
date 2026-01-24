namespace WebApiNibu.Helpers;

/// <summary>
/// Represents the result of an operation, encapsulating success/failure state,
/// a value (on success), and error messages (on failure).
/// </summary>
/// <typeparam name="T">The type of the value returned on success.</typeparam>
public class Result<T>
{
    public bool IsSuccess { get; private init; }
    public T? Value { get; private init; }
    public List<string> Errors { get; private init; } = [];

    private Result() { }

    public static Result<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Errors = [error]
    };

    public static Result<T> Failure(IEnumerable<string> errors) => new()
    {
        IsSuccess = false,
        Errors = errors.ToList()
    };
}
