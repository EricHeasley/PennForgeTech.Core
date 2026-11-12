using System;

namespace PennForgeTech.Core.Exceptions
{
    public class PennForgeTechException : Exception
    {
        public PennForgeTechException()
        {
        }

        public PennForgeTechException(string message)
            : base(message)
        {
        }

        public PennForgeTechException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
