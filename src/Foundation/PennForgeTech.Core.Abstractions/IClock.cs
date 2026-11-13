using System;

namespace PennForgeTech.Core.Abstractions
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
