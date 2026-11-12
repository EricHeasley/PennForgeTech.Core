using System;

namespace PennForgeTech.Core.Time;

/// <summary>An <see cref="IClock"/> that reports the real system time.</summary>
public sealed class SystemClock : IClock
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow
    {
        get { return DateTimeOffset.UtcNow; }
    }
}
