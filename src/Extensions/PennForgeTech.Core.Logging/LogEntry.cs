using System;
using PennForgeTech.Core.Abstractions;

namespace PennForgeTech.Core.Logging
{
    public sealed record LogEntry(DateTimeOffset Timestamp, LogLevel Level, string Message, Exception? Exception);
}
