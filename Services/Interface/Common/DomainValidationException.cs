namespace WebApiNibu.Services.Interface.Common;

public sealed class DomainValidationException : Exception
{
    public DomainValidationException(string message, IReadOnlyList<string> errors)
        : base(message)
    {
        Errors = errors;
    }

    public IReadOnlyList<string> Errors { get; }
}

