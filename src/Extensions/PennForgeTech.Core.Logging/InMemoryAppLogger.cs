using System;
using System.Collections.Generic;
using PennForgeTech.Core.Abstractions;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Logging
{
    public sealed class InMemoryAppLogger : IAppLogger
    {
        private readonly List<LogEntry> _entries = new List<LogEntry>();
        private readonly object _lock = new object();
        private readonly IClock _clock;

        public InMemoryAppLogger(IClock clock)
        {
            this._clock = Guard.AgainstNull(clock, nameof(clock));
        }

        public IReadOnlyList<LogEntry> Entries
        {
            get
            {
                lock (this._lock)
                {
                    return new List<LogEntry>(this._entries);
                }
            }
        }

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }

        public void Log(LogLevel level, string message, Exception? exception = null)
        {
            LogEntry entry = new LogEntry(this._clock.UtcNow, level, message, exception);

            lock (this._lock)
            {
                this._entries.Add(entry);
            }
        }
    }
}
