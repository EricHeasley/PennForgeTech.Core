using System;
using System.Collections.Generic;
using System.Linq;
using PennForgeTech.Core.Exceptions;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Validation
{
    public sealed class ValidationException : PennForgeTechException
    {
        public ValidationException()
            : this(Array.Empty<ValidationFailure>())
        {
        }

        public ValidationException(string message)
            : base(message)
        {
            this.Failures = Array.Empty<ValidationFailure>();
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.Failures = Array.Empty<ValidationFailure>();
        }

        public ValidationException(IReadOnlyList<ValidationFailure> failures)
            : base(BuildMessage(failures))
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
