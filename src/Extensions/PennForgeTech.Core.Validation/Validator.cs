using System;
using System.Collections.Generic;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Validation
{
    public abstract class Validator<TInstance> : IValidator<TInstance>
    {
        private readonly List<ValidationRule> _rules = new List<ValidationRule>();

        public ValidationResult Validate(TInstance instance)
        {
            Guard.AgainstNull(instance as object, nameof(instance));

            List<ValidationFailure> failures = new List<ValidationFailure>();

            foreach (ValidationRule rule in this._rules)
            {
                if (!rule.IsValid(instance))
                {
                    failures.Add(new ValidationFailure(rule.PropertyName, rule.Message));
                }
            }

            return new ValidationResult(failures);
        }

        protected void AddRule(string propertyName, Func<TInstance, bool> isValid, string message)
        {
            Guard.AgainstNullOrWhiteSpace(propertyName, nameof(propertyName));
            Guard.AgainstNull(isValid, nameof(isValid));
            Guard.AgainstNullOrWhiteSpace(message, nameof(message));

            this._rules.Add(new ValidationRule(propertyName, isValid, message));
        }

        private sealed class ValidationRule
        {
            public ValidationRule(string propertyName, Func<TInstance, bool> isValid, string message)
            {
                this.PropertyName = propertyName;
                this.IsValid = isValid;
                this.Message = message;
            }

            public string PropertyName { get; }

            public Func<TInstance, bool> IsValid { get; }

            public string Message { get; }
        }
    }
}
