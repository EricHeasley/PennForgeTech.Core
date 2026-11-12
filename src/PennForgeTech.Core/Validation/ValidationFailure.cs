using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Validation
{
    public sealed record ValidationFailure
    {
        public ValidationFailure(string propertyName, string message)
        {
            this.PropertyName = Guard.AgainstNullOrWhiteSpace(propertyName, nameof(propertyName));
            this.Message = Guard.AgainstNullOrWhiteSpace(message, nameof(message));
        }

        public string PropertyName { get; }

        public string Message { get; }
    }
}
