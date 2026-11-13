using System;
using PennForgeTech.Core.Abstractions;

namespace PennForgeTech.Core.Logging
{
    public sealed class NullAppLogger : IAppLogger
    {
        public static readonly NullAppLogger Instance = new NullAppLogger();

        private NullAppLogger()
        {
        }

        public bool IsEnabled(LogLevel level)
        {
            return false;
        }

        public void Log(LogLevel level, string message, Exception? exception = null)
        {
        }
    }
}
