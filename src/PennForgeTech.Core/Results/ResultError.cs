using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Results
{
    public sealed record ResultError
    {
        public ResultError(string code, string message)
        {
            this.Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code));
            this.Message = Guard.AgainstNullOrWhiteSpace(message, nameof(message));
        }

        public string Code { get; }

        public string Message { get; }
    }
}
