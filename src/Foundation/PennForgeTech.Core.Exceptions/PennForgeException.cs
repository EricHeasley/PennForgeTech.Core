using System;

namespace PennForgeTech.Core.Exceptions
{
    public class PennForgeException : Exception
    {
        public PennForgeException(string code, string message)
            : base(message)
        {
            this.Code = RequireCode(code);
        }

        public PennForgeException(string code, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Code = RequireCode(code);
        }

        public string Code { get; }

        private static string RequireCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code cannot be null, empty, or white space.", nameof(code));
            }

            return code;
        }
    }
}
