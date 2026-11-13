using System;
using PennForgeTech.Core.Exceptions;

namespace PennForgeTech.Core.Serialization.Json
{
    public sealed class SerializationFailedException : PennForgeException
    {
        public const string ErrorCode = "Serialization.Failed";

        public SerializationFailedException(string message)
            : base(ErrorCode, message)
        {
        }

        public SerializationFailedException(string message, Exception innerException)
            : base(ErrorCode, message, innerException)
        {
        }
    }
}
