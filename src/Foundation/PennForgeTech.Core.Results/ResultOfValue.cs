using System;

namespace PennForgeTech.Core.Results
{
    public sealed class Result<TValue> : Result
    {
        private readonly TValue? _value;

        internal Result(TValue? value, bool isSuccess, ResultError? error)
            : base(isSuccess, error)
        {
            this._value = value;
        }

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

        public Result<TNext> Map<TNext>(Func<TValue, TNext> transform)
        {
            if (this.IsFailure)
            {
                return Failure<TNext>(this.Error);
            }

            return Success(transform(this.Value));
        }

        public Result<TNext> Then<TNext>(Func<TValue, Result<TNext>> next)
        {
            if (this.IsFailure)
            {
                return Failure<TNext>(this.Error);
            }

            return next(this.Value);
        }

        public TValue ValueOr(TValue fallback)
        {
            if (this.IsFailure)
            {
                return fallback;
            }

            return this.Value;
        }
    }
}
