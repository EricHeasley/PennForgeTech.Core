using System;

namespace PennForgeTech.Core.Results;

/// <summary>The outcome of an operation that returns a value: either success with the value, or failure with an <see cref="ResultError"/>.</summary>
/// <typeparam name="TValue">The type of the value produced on success.</typeparam>
public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue? value, bool isSuccess, ResultError? error)
        : base(isSuccess, error)
    {
        this._value = value;
    }

    /// <summary>Gets the value produced by the operation.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is a failure.</exception>
    public TValue Value
    {
        get
        {
            if (this.IsFailure)
            {
                throw new InvalidOperationException("A failed result has no value.");
            }

            return this._value!;
        }
    }

    /// <summary>Transforms the value when the result is a success; otherwise passes the failure through.</summary>
    /// <param name="transform">The function that converts the value.</param>
    /// <typeparam name="TNext">The type of the transformed value.</typeparam>
    /// <returns>A result containing the transformed value, or the original failure.</returns>
    public Result<TNext> Map<TNext>(Func<TValue, TNext> transform)
    {
        if (this.IsFailure)
        {
            return Failure<TNext>(this.Error);
        }

        return Success(transform(this.Value));
    }

    /// <summary>Chains another result-producing step when this result is a success; otherwise passes the failure through.</summary>
    /// <param name="next">The step to run with the value.</param>
    /// <typeparam name="TNext">The type of the value produced by the next step.</typeparam>
    /// <returns>The result of the next step, or the original failure.</returns>
    public Result<TNext> Then<TNext>(Func<TValue, Result<TNext>> next)
    {
        if (this.IsFailure)
        {
            return Failure<TNext>(this.Error);
        }

        return next(this.Value);
    }

    /// <summary>Returns the value on success, or the supplied fallback on failure.</summary>
    /// <param name="fallback">The value to use when the result is a failure.</param>
    /// <returns>The value or the fallback.</returns>
    public TValue ValueOr(TValue fallback)
    {
        if (this.IsFailure)
        {
            return fallback;
        }

        return this.Value;
    }
}
