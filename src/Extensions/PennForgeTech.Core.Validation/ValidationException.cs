using System.Collections.Generic;
using System.Linq;
using PennForgeTech.Core.Exceptions;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Validation
{
    public sealed class ValidationException : PennForgeException
    {
        public const string ErrorCode = "Validation.Failed";

        public ValidationException(IReadOnlyList<ValidationFailure> failures)
            : base(ErrorCode, BuildMessage(failures))
        {
            this.Failures = failures;
        }

        public IReadOnlyList<ValidationFailure> Failures { get; }

        private static string BuildMessage(IReadOnlyList<ValidationFailure> failures)
        {
            Guard.AgainstNull(failures, nameof(failures));

            IEnumerable<string> lines = failures.Select(failure => $"{failure.PropertyName}: {failure.Message}");
            return "Validation failed. " + string.Join(" ", lines);
        }
    }
}
