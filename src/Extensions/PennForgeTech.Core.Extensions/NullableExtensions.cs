using PennForgeTech.Core.Primitives;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Core.Extensions
{
    public static class NullableExtensions
    {
        public static Result<TValue> ToResult<TValue>(this TValue? value, ResultError errorWhenNull)
            where TValue : class
        {
            Guard.AgainstNull(errorWhenNull, nameof(errorWhenNull));

            if (value is null)
            {
                return Result.Failure<TValue>(errorWhenNull);
            }

            return Result.Success(value);
        }
    }
}
