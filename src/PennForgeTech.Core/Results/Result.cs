using System;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Results
{
    public class Result
    {
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

        public bool IsSuccess { get; }

        public bool IsFailure
        {
            get { return !this.IsSuccess; }
        }

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

        public static Result Success()
        {
            return new Result(isSuccess: true, error: null);
        }

        public static Result Failure(ResultError error)
        {
            Guard.AgainstNull(error, nameof(error));
            return new Result(isSuccess: false, error: error);
        }

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(value, isSuccess: true, error: null);
        }

        public static Result<TValue> Failure<TValue>(ResultError error)
        {
            Guard.AgainstNull(error, nameof(error));
            return new Result<TValue>(default, isSuccess: false, error: error);
        }
    }
}
