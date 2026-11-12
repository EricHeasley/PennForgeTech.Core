using System;
using PennForgeTech.Core.Exceptions;

namespace PennForgeTech.Core.Serialization
{
    public sealed class SerializationFailedException : PennForgeTechException
    {
        public SerializationFailedException()
        {
        }

        public SerializationFailedException(string message)
            : base(message)
        {
        }

        public SerializationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
