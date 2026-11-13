using System;

namespace PennForgeTech.Core.Abstractions
{
    public interface IAppLogger
    {
        bool IsEnabled(LogLevel level);

        void Log(LogLevel level, string message, Exception? exception = null);
    }
}
