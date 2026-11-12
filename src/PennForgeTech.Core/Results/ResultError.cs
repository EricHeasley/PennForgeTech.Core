using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Results;

/// <summary>Describes why an operation failed.</summary>
public sealed record ResultError
{
    /// <summary>Initializes a new instance of the <see cref="ResultError"/> record.</summary>
    /// <param name="code">A short, stable, machine-readable identifier such as "User.NotFound".</param>
    /// <param name="message">A human-readable explanation of the failure.</param>
    public ResultError(string code, string message)
    {
        this.Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code));
        this.Message = Guard.AgainstNullOrWhiteSpace(message, nameof(message));
    }

    /// <summary>Gets the short, stable, machine-readable identifier of the error.</summary>
    public string Code { get; }

    /// <summary>Gets the human-readable explanation of the failure.</summary>
    public string Message { get; }
}
