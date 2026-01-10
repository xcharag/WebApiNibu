namespace WebApiNibu.Services.Interface.Common;

/// <summary>
/// Lightweight result type for CQRS commands, to avoid using exceptions for validation/control-flow.
/// </summary>
public record Result
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

    public static Result Ok(string? message = null) => new() { IsSuccess = true, Message = message };

    public static Result Fail(string message, IReadOnlyList<string>? errors = null) => new()
    {
        IsSuccess = false,
        Message = message,
        Errors = errors ?? Array.Empty<string>()
    };
}

public record Result<T> : Result
{
    public T? Value { get; init; }

    public static Result<T> Ok(T value, string? message = null) => new()
    {
        IsSuccess = true,
        Value = value,
        Message = message
    };

    public static new Result<T> Fail(string message, IReadOnlyList<string>? errors = null) => new()
    {
        IsSuccess = false,
        Message = message,
        Errors = errors ?? Array.Empty<string>()
    };
}

