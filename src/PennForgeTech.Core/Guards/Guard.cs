using System;
using System.Collections.Generic;
using System.Linq;

namespace PennForgeTech.Core.Guards;

/// <summary>
/// Argument validation helpers. Each method throws a descriptive exception
/// when the value is invalid, and returns the value when it is valid.
/// </summary>
public static class Guard
{
    /// <summary>Throws <see cref="ArgumentNullException"/> when the value is null.</summary>
    /// <param name="value">The value to check.</param>
    /// <param name="parameterName">The name of the parameter being checked.</param>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <returns>The value, guaranteed not to be null.</returns>
    public static TValue AgainstNull<TValue>(TValue? value, string parameterName)
        where TValue : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    /// <summary>Throws <see cref="ArgumentException"/> when the text is null, empty, or only white space.</summary>
    /// <param name="value">The text to check.</param>
    /// <param name="parameterName">The name of the parameter being checked.</param>
    /// <returns>The text, guaranteed to contain at least one visible character.</returns>
    public static string AgainstNullOrWhiteSpace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null, empty, or white space.", parameterName);
        }

        return value;
    }

    /// <summary>Throws <see cref="ArgumentException"/> when the collection is null or has no items.</summary>
    /// <param name="values">The collection to check.</param>
    /// <param name="parameterName">The name of the parameter being checked.</param>
    /// <typeparam name="TItem">The type of item in the collection.</typeparam>
    /// <returns>The collection, guaranteed to contain at least one item.</returns>
    public static IReadOnlyCollection<TItem> AgainstNullOrEmpty<TItem>(
        IReadOnlyCollection<TItem>? values,
        string parameterName)
    {
        if (values is null || values.Count == 0)
        {
            throw new ArgumentException("Collection cannot be null or empty.", parameterName);
        }

        return values;
    }

    /// <summary>Throws <see cref="ArgumentOutOfRangeException"/> when the value is below the minimum.</summary>
    /// <param name="value">The value to check.</param>
    /// <param name="minimum">The smallest allowed value.</param>
    /// <param name="parameterName">The name of the parameter being checked.</param>
    /// <typeparam name="TValue">The comparable type of the value.</typeparam>
    /// <returns>The value, guaranteed to be at least the minimum.</returns>
    public static TValue AgainstBelowMinimum<TValue>(TValue value, TValue minimum, string parameterName)
        where TValue : IComparable<TValue>
    {
        if (value.CompareTo(minimum) < 0)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                value,
                $"Value must be at least {minimum}.");
        }

        return value;
    }

    /// <summary>Throws <see cref="ArgumentOutOfRangeException"/> when the value is outside the inclusive range.</summary>
    /// <param name="value">The value to check.</param>
    /// <param name="minimum">The smallest allowed value.</param>
    /// <param name="maximum">The largest allowed value.</param>
    /// <param name="parameterName">The name of the parameter being checked.</param>
    /// <typeparam name="TValue">The comparable type of the value.</typeparam>
    /// <returns>The value, guaranteed to be within the range.</returns>
    public static TValue AgainstOutOfRange<TValue>(
        TValue value,
        TValue minimum,
        TValue maximum,
        string parameterName)
        where TValue : IComparable<TValue>
    {
        if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                value,
                $"Value must be between {minimum} and {maximum}.");
        }

        return value;
    }

    /// <summary>Throws <see cref="ArgumentException"/> when the collection contains a null item.</summary>
    /// <param name="values">The collection to check.</param>
    /// <param name="parameterName">The name of the parameter being checked.</param>
    /// <typeparam name="TItem">The type of item in the collection.</typeparam>
    /// <returns>The collection, guaranteed to contain no null items.</returns>
    public static IReadOnlyCollection<TItem> AgainstNullItems<TItem>(
        IReadOnlyCollection<TItem> values,
        string parameterName)
        where TItem : class
    {
        AgainstNull(values, parameterName);

        if (values.Any(item => item is null))
        {
            throw new ArgumentException("Collection cannot contain null items.", parameterName);
        }

        return values;
    }
}
