using System;
using System.IO;
using PennForgeTech.Core.Abstractions;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Logging
{
    public sealed class ConsoleAppLogger : IAppLogger
    {
        private readonly IClock _clock;
        private readonly LogLevel _minimumLevel;
        private readonly TextWriter _writer;
        private readonly object _lock = new object();

        public ConsoleAppLogger(IClock clock, LogLevel minimumLevel = LogLevel.Information, TextWriter? writer = null)
        {
            this._clock = Guard.AgainstNull(clock, nameof(clock));
            this._minimumLevel = minimumLevel;
            this._writer = writer ?? Console.Out;
        }

        public bool IsEnabled(LogLevel level)
        {
            return level >= this._minimumLevel;
        }

        public void Log(LogLevel level, string message, Exception? exception = null)
        {
            if (!this.IsEnabled(level))
            {
                return;
            }

            string timestamp = this._clock.UtcNow.ToString("O");
            string line = $"{timestamp} [{level}] {message}";

            lock (this._lock)
            {
                this._writer.WriteLine(line);

                if (exception is not null)
                {
                    this._writer.WriteLine(exception.ToString());
                }
            }
        }
    }
}
