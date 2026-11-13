using System;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Abstractions
{
    public static class AppLoggerExtensions
    {
        public static void LogTrace(this IAppLogger logger, string message)
        {
            Guard.AgainstNull(logger, nameof(logger)).Log(LogLevel.Trace, message);
        }

        public static void LogDebug(this IAppLogger logger, string message)
        {
            Guard.AgainstNull(logger, nameof(logger)).Log(LogLevel.Debug, message);
        }

        public static void LogInformation(this IAppLogger logger, string message)
        {
            Guard.AgainstNull(logger, nameof(logger)).Log(LogLevel.Information, message);
        }

        public static void LogWarning(this IAppLogger logger, string message)
        {
            Guard.AgainstNull(logger, nameof(logger)).Log(LogLevel.Warning, message);
        }

        public static void LogWarning(this IAppLogger logger, Exception exception, string message)
        {
            Guard.AgainstNull(logger, nameof(logger)).Log(LogLevel.Warning, message, exception);
        }

        public static void LogError(this IAppLogger logger, string message)
        {
            Guard.AgainstNull(logger, nameof(logger)).Log(LogLevel.Error, message);
        }

        public static void LogError(this IAppLogger logger, Exception exception, string message)
        {
            Guard.AgainstNull(logger, nameof(logger)).Log(LogLevel.Error, message, exception);
        }

        public static void LogCritical(this IAppLogger logger, Exception exception, string message)
        {
            Guard.AgainstNull(logger, nameof(logger)).Log(LogLevel.Critical, message, exception);
        }
    }
}
