using System.Collections.Generic;
using PennForgeTech.Core.Primitives;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Core.Validation
{
    public sealed class ValidationResult
    {
        public ValidationResult(IReadOnlyList<ValidationFailure> failures)
        {
            this.Failures = Guard.AgainstNull(failures, nameof(failures));
        }

        public IReadOnlyList<ValidationFailure> Failures { get; }

        public bool IsValid
        {
            get { return this.Failures.Count == 0; }
        }

        public void ThrowIfInvalid()
        {
            if (!this.IsValid)
            {
                throw new ValidationException(this.Failures);
            }
        }

        public Result ToResult()
        {
            if (this.IsValid)
            {
                return Result.Success();
            }

            string message = new ValidationException(this.Failures).Message;
            return Result.Failure(new ResultError(ValidationException.ErrorCode, message));
        }
    }
}
