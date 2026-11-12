using System;

namespace PennForgeTech.Core.Time
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
