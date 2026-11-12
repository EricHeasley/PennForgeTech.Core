using System;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Results;

/// <summary>The outcome of an operation that returns no value: either success, or failure with an <see cref="ResultError"/>.</summary>
public class Result
{
    /// <summary>Initializes a new instance of the <see cref="Result"/> class.</summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="error">The failure reason. Required for failures, and must be null for successes.</param>
    protected Result(bool isSuccess, ResultError? error)
    {
        if (isSuccess && error is not null)
        {
            throw new ArgumentException("A successful result cannot carry an error.", nameof(error));
        }

        if (!isSuccess && error is null)
        {
            throw new ArgumentException("A failed result must carry an error.", nameof(error));
        }

        this.IsSuccess = isSuccess;
        this.ErrorOrNull = error;
    }

    /// <summary>Gets a value indicating whether the operation succeeded.</summary>
    public bool IsSuccess { get; }

    /// <summary>Gets a value indicating whether the operation failed.</summary>
    public bool IsFailure
    {
        get { return !this.IsSuccess; }
    }

    /// <summary>Gets the failure reason.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is a success.</exception>
    public ResultError Error
    {
        get
        {
            if (this.ErrorOrNull is null)
            {
                throw new InvalidOperationException("A successful result has no error.");
            }

            return this.ErrorOrNull;
        }
    }

    private ResultError? ErrorOrNull { get; }

    /// <summary>Creates a successful result with no value.</summary>
    /// <returns>A successful result.</returns>
    public static Result Success()
    {
        return new Result(isSuccess: true, error: null);
    }

    /// <summary>Creates a failed result.</summary>
    /// <param name="error">The reason for the failure.</param>
    /// <returns>A failed result.</returns>
    public static Result Failure(ResultError error)
    {
        Guard.AgainstNull(error, nameof(error));
        return new Result(isSuccess: false, error: error);
    }

    /// <summary>Creates a successful result carrying a value.</summary>
    /// <param name="value">The value produced by the operation.</param>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <returns>A successful result containing the value.</returns>
    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, isSuccess: true, error: null);
    }

    /// <summary>Creates a failed result for an operation that would have produced a value.</summary>
    /// <param name="error">The reason for the failure.</param>
    /// <typeparam name="TValue">The type of the value that was expected.</typeparam>
    /// <returns>A failed result.</returns>
    public static Result<TValue> Failure<TValue>(ResultError error)
    {
        Guard.AgainstNull(error, nameof(error));
        return new Result<TValue>(default, isSuccess: false, error: error);
    }
}
