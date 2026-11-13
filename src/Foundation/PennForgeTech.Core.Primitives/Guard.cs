using System;
using System.Collections.Generic;
using System.Linq;

namespace PennForgeTech.Core.Primitives
{
    public static class Guard
    {
        public static TValue AgainstNull<TValue>(TValue? value, string parameterName)
            where TValue : class
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string AgainstNullOrWhiteSpace(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null, empty, or white space.", parameterName);
            }

            return value;
        }

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
}
