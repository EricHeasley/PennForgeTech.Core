using System;

namespace PennForgeTech.Core.Time;

/// <summary>An <see cref="IClock"/> whose time is set manually. Intended for tests.</summary>
public sealed class FixedClock : IClock
{
    /// <summary>Initializes a new instance of the <see cref="FixedClock"/> class.</summary>
    /// <param name="utcNow">The time the clock initially reports.</param>
    public FixedClock(DateTimeOffset utcNow)
    {
        this.UtcNow = utcNow;
    }

    /// <inheritdoc />
    public DateTimeOffset UtcNow { get; private set; }

    /// <summary>Moves the clock forward by the given amount.</summary>
    /// <param name="amount">How far to advance the clock.</param>
    public void Advance(TimeSpan amount)
    {
        this.UtcNow = this.UtcNow.Add(amount);
    }
}
