using System;
using Microsoft.Extensions.Logging;
using PennForgeTech.Core.Abstractions;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Logging.MicrosoftExtensions
{
    public sealed class MicrosoftAppLogger : IAppLogger
    {
        private readonly ILogger _logger;

        public MicrosoftAppLogger(ILogger logger)
        {
            this._logger = Guard.AgainstNull(logger, nameof(logger));
        }

        public bool IsEnabled(PennForgeTech.Core.Abstractions.LogLevel level)
        {
            return this._logger.IsEnabled(ConvertLevel(level));
        }

        public void Log(PennForgeTech.Core.Abstractions.LogLevel level, string message, Exception? exception = null)
        {
            this._logger.Log(ConvertLevel(level), default(EventId), message, exception, FormatMessage);
        }

        private static string FormatMessage(string message, Exception? exception)
        {
            return message;
        }

        private static Microsoft.Extensions.Logging.LogLevel ConvertLevel(PennForgeTech.Core.Abstractions.LogLevel level)
        {
            switch (level)
            {
                case PennForgeTech.Core.Abstractions.LogLevel.Trace:
                    return Microsoft.Extensions.Logging.LogLevel.Trace;
                case PennForgeTech.Core.Abstractions.LogLevel.Debug:
                    return Microsoft.Extensions.Logging.LogLevel.Debug;
                case PennForgeTech.Core.Abstractions.LogLevel.Information:
                    return Microsoft.Extensions.Logging.LogLevel.Information;
                case PennForgeTech.Core.Abstractions.LogLevel.Warning:
                    return Microsoft.Extensions.Logging.LogLevel.Warning;
                case PennForgeTech.Core.Abstractions.LogLevel.Error:
                    return Microsoft.Extensions.Logging.LogLevel.Error;
                case PennForgeTech.Core.Abstractions.LogLevel.Critical:
                    return Microsoft.Extensions.Logging.LogLevel.Critical;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "Unknown log level.");
            }
        }
    }
}
