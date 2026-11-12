using System;

namespace PennForgeTech.Core.Time;

/// <summary>Provides the current time. Depend on this instead of <see cref="DateTime.UtcNow"/> so time can be controlled in tests.</summary>
public interface IClock
{
    /// <summary>Gets the current moment in time, in UTC.</summary>
    DateTimeOffset UtcNow { get; }
}
